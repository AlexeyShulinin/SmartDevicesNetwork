using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Mappings;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class DevicesRepository(SdnDbContext dbContext) : IDevicesRepository
{
    public Task<List<DevicesDtoModel>> ListAsync(CancellationToken cancellationToken)
        => dbContext.Devices.ProjectToListDto().ToListAsync(cancellationToken);

    public Task<DeviceDtoModel> ByIdAsync(int deviceId, CancellationToken cancellationToken)
        => dbContext.Devices
            .ProjectToDto()
            .FirstAsync(x => x.DeviceId == deviceId, cancellationToken);

    public void Update(DeviceDtoModel device, string message)
    {
        dbContext.Update(device.MapToEntity());
        dbContext.DeviceLogs.Add(new DeviceLog
        {
            DeviceId = device.DeviceId,
            Message = message,
            TimeStamp = DateTimeOffset.UtcNow
        });
    }
}