using System;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public record DevicesResponse(int Id, string Name, string Type, string Status, DateTime LastActive);