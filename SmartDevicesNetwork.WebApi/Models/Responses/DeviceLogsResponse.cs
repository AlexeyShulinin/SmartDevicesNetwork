using System;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public class DeviceLogsResponse
{
    public BaseEntityResponse Device { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string Message { get; set; }
}