using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class NetworkRepository(SdnDbContext dbContext, IMapper mapper) : INetworkRepository
{
    public Task<List<NetworkLinkDtoModel>> ListAsync(CancellationToken cancellationToken)
        => dbContext.NetworkLinks.ProjectTo<NetworkLinkDtoModel>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
}