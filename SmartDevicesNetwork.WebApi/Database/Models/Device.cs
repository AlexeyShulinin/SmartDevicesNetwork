using System;

namespace SmartDevicesNetwork.WebApi.Database.Models;

public class Device
{
    public int DeviceId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime LastActive { get; set; }
    public string IpAddress { get; set; }
    public string FirmwareVersion { get; set; }
    public int? BatteryLevel { get; set; }
}