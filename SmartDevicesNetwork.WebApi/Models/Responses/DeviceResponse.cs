using System;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public record DeviceResponse(int Id, string Name, string Type, string Status, DateTime LastActive, DeviceDetailsResponse Details);