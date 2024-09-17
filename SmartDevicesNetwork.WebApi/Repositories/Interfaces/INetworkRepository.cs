using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Interfaces;

public interface INetworkRepository
{
    Task<List<NetworkLinkDtoModel>> ListAsync(CancellationToken cancellationToken);
}