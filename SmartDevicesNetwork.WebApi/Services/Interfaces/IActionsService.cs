using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;

namespace SmartDevicesNetwork.WebApi.Services.Interfaces;

public interface IActionsService
{
    Task<ActionResponse> PerformActionAsync(int deviceId, ActionRequest actionRequest, CancellationToken cancellationToken);
}