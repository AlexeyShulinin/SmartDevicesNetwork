using System;

namespace SmartDevicesNetwork.WebApi.Database.Models;

public class Action
{
    public int ActionId { get; set; }
    public int DeviceId { get; set; }
    public string ActionType { get; set; }
    public DateTimeOffset TimeStamp { get; set; }

    public Device Device { get; set; }
}