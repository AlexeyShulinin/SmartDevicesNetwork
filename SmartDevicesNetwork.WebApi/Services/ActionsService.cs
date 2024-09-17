using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services.Interfaces;
using SmartDevicesNetwork.WebApi.Shared;

namespace SmartDevicesNetwork.WebApi.Services;

public class ActionsService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : IActionsService
{
    private readonly Dictionary<string, ActionDetails> actions = new()
    {
        { "on", new(Statuses.Online, "Device switched on successfully", "Unable to switch on device") },
        { "off", new(Statuses.Offline, "Device switched off successfully", "Unable to switch off device") },
        { "reboot", new(Statuses.Rebooting, "Device rebooted successfully", "Unable to reboot device") }
    };
    
    public async Task<ActionResponse> PerformActionAsync(int deviceId, ActionRequest actionRequest, CancellationToken cancellationToken)
    {
        var action = actions.GetValueOrDefault(actionRequest.Action);
        if (action == default)
        {
            return new ActionResponse("Failed", $"Action {actionRequest.Action} was not found");
        }
        
        var dbDevice = await unitOfWork.DevicesRepository.ByIdAsync(deviceId, cancellationToken);
        if (dbDevice == null)
        {
            return new ActionResponse("Failed", "Device wasn't found");
        }
        
        await Task.Delay(1000 * 30, cancellationToken);

        var countOfRequests = await CountOfRequestsAsync();
        if (countOfRequests % 10 == 0)
        {
            return await PerformAction("Failed", action.FailureMessage);
        }
        
        dbDevice.Status = action.Status;
        return await PerformAction("Success", action.SuccessMessage);

        async Task<ActionResponse> PerformAction(string status, string message)
        {
            unitOfWork.DevicesRepository.Update(dbDevice, message);
            unitOfWork.ActionsRepository.Add(new ActionsDtoModel { DeviceId = deviceId, ActionType = actionRequest.Action });
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ActionResponse(status, message);
        }
    }

    private Task<int> CountOfRequestsAsync()
        => memoryCache.GetOrCreateAsync(CacheConstants.ActionRequestCountKey,
            cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return Task.FromResult(0);
            });

    private record ActionDetails(string Status, string SuccessMessage, string FailureMessage);
}