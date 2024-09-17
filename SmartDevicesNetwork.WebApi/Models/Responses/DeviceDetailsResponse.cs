namespace SmartDevicesNetwork.WebApi.Models.Responses;

public record DeviceDetailsResponse(string Ip, string FirmwareVersion, int? BatteryLevel);