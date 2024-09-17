﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Shared;

namespace SmartDevicesNetwork.WebApi.BackgroundServices;

public class NetworkEmulator(ILogger<NetworkEmulator> logger, IServiceProvider serviceProvider) : BackgroundService
{
    private int countOfExecutions;
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<SdnDbContext>();

        while (true)
        {
            await Task.Delay(1000 * 60 * 1, cancellationToken);

            var devices = await dbContext.Devices.ToListAsync(cancellationToken);
            
            var devicesToReboot = devices.Where(x => x.Status == Statuses.Rebooting);
            RebootDevices(devicesToReboot, dbContext);

            if (countOfExecutions % 100 == 0)
            {
                TurnOffRandomDevice(devices, dbContext);
            }

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e.Message);
            }

            countOfExecutions++;
        }
    }

    private void RebootDevices(IEnumerable<Device> devices, SdnDbContext dbContext)
    {
        logger.LogInformation("Start rebooting...");
        foreach (var device in devices)
        {
            device.Status = Statuses.Online;

            dbContext.DeviceLogs.Add(new DeviceLog()
            {
                DeviceId = device.DeviceId,
                TimeStamp = DateTime.UtcNow,
                Message = "Device rebooted successfully"
            });
        }
    }

    private void TurnOffRandomDevice(List<Device> devices, SdnDbContext dbContext)
    {
        var randomDevice = devices[new Random().Next(1, devices.Count)];
        randomDevice.Status = Statuses.Offline;
        
        dbContext.DeviceLogs.Add(new DeviceLog()
        {
            DeviceId = randomDevice.DeviceId,
            TimeStamp = DateTime.UtcNow,
            Message = "Device switched off"
        });
    }
}