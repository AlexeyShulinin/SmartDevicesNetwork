using System;
using System.Collections.Generic;
using System.Linq;
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

    public Task<PagedListDto<DevicesLogsDto>> LogsListByDeviceIdAsync(int deviceId, PageFilterDto filter, CancellationToken cancellationToken)
        => LogsListAsync(dbContext.DeviceLogs.Where(x => x.DeviceId == deviceId), filter, cancellationToken);

    public Task<PagedListDto<DevicesLogsDto>> LogsListAsync(PageFilterDto filter, CancellationToken cancellationToken)
        => LogsListAsync(dbContext.DeviceLogs, filter, cancellationToken);

    private async Task<PagedListDto<DevicesLogsDto>> LogsListAsync(IQueryable<DeviceLog> query, PageFilterDto filter, CancellationToken cancellationToken)
    {
        var deviceLogs = await query
            .OrderByDescending(x => x.TimeStamp)
            .Skip(filter.Limit * filter.Page)
            .Take(filter.Limit)
            .ProjectToDto()
            .ToListAsync(cancellationToken);

        var totalItemsCount = await query
            .CountAsync(cancellationToken);

        return new PagedListDto<DevicesLogsDto>(deviceLogs, filter.Page, totalItemsCount);
    }
}