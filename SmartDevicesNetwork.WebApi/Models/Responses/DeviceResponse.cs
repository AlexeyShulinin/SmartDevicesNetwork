﻿using System;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public class DeviceResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTimeOffset LastActive { get; set; }
    public DeviceDetailsResponse Details { get; set; }
}