using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using Action = SmartDevicesNetwork.WebApi.Database.Models.Action;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class ActionsRepository(SdnDbContext dbContext, IMapper mapper) : IActionsRepository
{
    public Task<List<ActionsDtoModel>> GetDeviceActionsAsync(int deviceId, CancellationToken cancellationToken)
        => dbContext.Actions
            .Where(x => x.DeviceId == deviceId).ProjectTo<ActionsDtoModel>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public Task<ActionsDtoModel> ActionAsync(string actionType, CancellationToken cancellationToken)
        => dbContext.Actions
            .ProjectTo<ActionsDtoModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.ActionType == actionType, cancellationToken);

    public void Add(ActionsDtoModel action)
    {
        var dbAction = mapper.Map<Action>(action);
        dbAction.TimeStamp = DateTime.UtcNow;
        dbContext.Add(dbAction);
    }
}