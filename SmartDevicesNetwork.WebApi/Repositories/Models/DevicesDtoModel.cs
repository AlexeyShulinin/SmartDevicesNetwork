using System;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class DevicesDtoModel : DeviceBaseDtoModel
{
    public DateTimeOffset LastActive { get; set; }
}