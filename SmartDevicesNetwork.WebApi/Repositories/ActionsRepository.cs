using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Mappings;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using Action = SmartDevicesNetwork.WebApi.Database.Models.Action;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class ActionsRepository(SdnDbContext dbContext) : IActionsRepository
{
    public Task<List<ActionsDtoModel>> GetDeviceActionsAsync(int deviceId, CancellationToken cancellationToken)
        => dbContext.Actions
            .Where(x => x.DeviceId == deviceId)
            .ProjectToDto()
            .ToListAsync(cancellationToken);

    public Task<ActionsDtoModel> ActionAsync(string actionType, CancellationToken cancellationToken)
        => dbContext.Actions
            .ProjectToDto()
            .FirstOrDefaultAsync(x => x.ActionType == actionType, cancellationToken);

    public void Add(ActionsDtoModel action)
    {
        var dbAction = action.MapToEntity();
        dbAction.TimeStamp = DateTimeOffset.UtcNow;
        dbContext.Add(dbAction);
    }
}