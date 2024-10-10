namespace SmartDevicesNetwork.WebApi.Models.Responses;

public class DeviceDetailsResponse
{
    public string Ip { get; set; }
    public string FirmwareVersion { get; set; }
    public int? BatteryLevel { get; set; }
}