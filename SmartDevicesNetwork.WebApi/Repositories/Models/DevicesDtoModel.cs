using System;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class DevicesDtoModel : DeviceBaseDtoModel
{
    public DateTime LastActive { get; set; }
}