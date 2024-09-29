using System;

namespace SmartDevicesNetwork.WebApi.Database.Models;

public class DeviceLog
{
    public int LogId { get; set; }
    public int DeviceId { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string Message { get; set; }

    public Device Device { get; set; }
}