using System;

namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class ActionsDtoModel
{
    public int ActionId { get; set; }
    public int DeviceId { get; set; }
    public string ActionType { get; set; }
    public DateTime TimeStamp { get; private set; }
}