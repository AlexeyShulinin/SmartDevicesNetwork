using System;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class DevicesLogsDto
{
    public BaseEntityDto Device { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string Message { get; set; }
}