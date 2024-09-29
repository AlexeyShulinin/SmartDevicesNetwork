using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class DevicesEndpoint
{
    public static void RegisterDevicesEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/")
            .WithOpenApi();
        
        api.MapGet("devices", DevicesAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get list of devices",
                Description = "Retrieve a list of all devices."
            })
            .Produces<List<DevicesResponse>>();
        
        api.MapGet("devices/{id}", DeviceByIdAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get device details",
                Description = "Retrieve detailed information about a specific device by its ID."
            })
            .Produces<DeviceResponse>();
        
        api.MapPost("devices/{id}/action", PerformActionAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Perform action",
                Description = "Perform an action on a device (turn on, reboot, etc.)."
            })
            .Produces<ActionResponse>();
    }

    private static Task<List<DevicesResponse>> DevicesAsync(IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.DevicesListAsync(cancellationToken);

    private static Task<DeviceResponse> DeviceByIdAsync(int id, IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.GetDeviceByIdAsync(id, cancellationToken);

    private static Task<ActionResponse> PerformActionAsync(int id, ActionRequest actionRequest, IActionsService actionService, CancellationToken cancellationToken)
        => actionService.PerformActionAsync(id, actionRequest, cancellationToken);
}