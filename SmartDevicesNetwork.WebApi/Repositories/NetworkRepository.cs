using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Mappings;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class NetworkRepository(SdnDbContext dbContext) : INetworkRepository
{
    public Task<List<NetworkLinkDtoModel>> ListAsync(CancellationToken cancellationToken)
        => dbContext.NetworkLinks.ProjectToDto().ToListAsync(cancellationToken);
}