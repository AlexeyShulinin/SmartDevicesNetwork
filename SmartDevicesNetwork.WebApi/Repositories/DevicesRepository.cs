using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class DevicesRepository(SdnDbContext dbContext, IMapper mapper) : IDevicesRepository
{
    public Task<List<DevicesDtoModel>> ListAsync(CancellationToken cancellationToken)
        => dbContext.Devices.ProjectTo<DevicesDtoModel>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);

    public Task<DeviceDtoModel> ByIdAsync(int deviceId, CancellationToken cancellationToken)
        => dbContext.Devices.ProjectTo<DeviceDtoModel>(mapper.ConfigurationProvider)
            .FirstAsync(x => x.DeviceId == deviceId, cancellationToken);

    public void Update(DeviceDtoModel device, string message)
    {
        var dbDevice = mapper.Map<Device>(device);
        dbContext.Update(dbDevice);
    }
}