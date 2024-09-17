using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Interfaces;

public interface IActionsRepository
{
    Task<List<ActionsDtoModel>> GetDeviceActionsAsync(int deviceId, CancellationToken cancellationToken);
    Task<ActionsDtoModel> ActionAsync(string actionType, CancellationToken cancellationToken);
    void Add(ActionsDtoModel action);
}