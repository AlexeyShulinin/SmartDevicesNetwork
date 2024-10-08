using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Enums;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Resources;
using SmartDevicesNetwork.WebApi.Services.Interfaces;
using SmartDevicesNetwork.WebApi.Shared;


namespace SmartDevicesNetwork.WebApi.Services;

public class ActionsService(
    IUnitOfWork unitOfWork,
    IMemoryCache memoryCache,
    IStringLocalizer<ApiMessages> apiMessagesLocalizer) : IActionsService
{
    private readonly Dictionary<Actions, ActionDetails> actions = new()
    {
        { Actions.On, new(Statuses.Online, apiMessagesLocalizer[ApiMessages.DeviceSwitchedOnSuccessMessage], apiMessagesLocalizer[ApiMessages.DeviceSwitchedOnErrorMessage]) },
        { Actions.Off, new(Statuses.Offline, apiMessagesLocalizer[ApiMessages.DeviceSwitchedOffSuccessMessage], apiMessagesLocalizer[ApiMessages.DeviceSwitchedOffErrorMessage]) },
        { Actions.Reboot, new(Statuses.Rebooting, apiMessagesLocalizer[ApiMessages.DeviceRebootSuccessMessage], apiMessagesLocalizer[ApiMessages.DeviceRebootErrorMessage]) }
    };
    
    public async Task<ActionResponse> PerformActionAsync(int deviceId, ActionRequest actionRequest, CancellationToken cancellationToken)
    {
        var action = actions.GetValueOrDefault(actionRequest.Action);
        if (action == default)
        {
            return new ActionResponse("Failed", string.Format(apiMessagesLocalizer[ApiMessages.ActionNotFoundErrorMessage], actionRequest.Action));
        }
        
        var dbDevice = await unitOfWork.DevicesRepository.ByIdAsync(deviceId, cancellationToken);
        if (dbDevice == null)
        {
            return new ActionResponse("Failed", apiMessagesLocalizer[ApiMessages.DeviceNotFoundErrorMessage]);
        }
        
        await Task.Delay(1000 * 5, cancellationToken);

        var countOfRequests = await CountOfRequestsAsync();
        if (countOfRequests % 10 == 0)
        {
            SetCountOfRequests(countOfRequests + 1);
            return await PerformAction("Failed", action.FailureMessage);
        }
        
        dbDevice.Status = Enum.GetName(action.Status);
        return await PerformAction("Success", action.SuccessMessage);

        async Task<ActionResponse> PerformAction(string status, string message)
        {
            unitOfWork.DevicesRepository.Update(dbDevice, message);
            unitOfWork.ActionsRepository.Add(new ActionsDtoModel { DeviceId = deviceId, ActionType = Enum.GetName(actionRequest.Action) });
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

    private void SetCountOfRequests(int countOfRequests)
        => memoryCache.Set(CacheConstants.ActionRequestCountKey, countOfRequests);

    private record ActionDetails(Statuses Status, string SuccessMessage, string FailureMessage);
}