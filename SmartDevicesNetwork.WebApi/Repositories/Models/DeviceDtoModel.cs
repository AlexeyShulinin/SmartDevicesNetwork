using System;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class DeviceDtoModel : DeviceBaseDtoModel
{
    public DateTime LastActive { get; set; }
    public string IpAddress { get; set; }
    public string FirmwareVersion { get; set; }
    public int? BatteryLevel { get; set; }
}