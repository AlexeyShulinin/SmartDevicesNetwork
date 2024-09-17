using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Models.Responses;

namespace SmartDevicesNetwork.WebApi.Services.Interfaces;

public interface INetworkService
{
    Task<NetworkResponse> ListAsync(CancellationToken cancellationToken);
}