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
            .Produces<ActionResponse>()
            .AddEndpointFilter<ValidationFilter<ActionRequest>>();
        
        api.MapPost("devices/{id}/logs", LogsByDeviceIdAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get device logs",
                Description = "Retrieve device logs."
            })
            .Produces<PagedListResponse<DeviceLogsResponse>>();
        
        api.MapPost("devices/logs", LogsAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get all device logs",
                Description = "Retrieve device logs."
            })
            .Produces<PagedListResponse<DeviceLogsResponse>>();
    }

    private static Task<List<DevicesResponse>> DevicesAsync(IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.DevicesListAsync(cancellationToken);

    private static Task<DeviceResponse> DeviceByIdAsync(int id, IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.GetDeviceByIdAsync(id, cancellationToken);

    private static Task<ActionResponse> PerformActionAsync(int id, ActionRequest actionRequest, IActionsService actionService, CancellationToken cancellationToken)
        => actionService.PerformActionAsync(id, actionRequest, cancellationToken);
    
    private static Task<PagedListResponse<DeviceLogsResponse>> LogsByDeviceIdAsync(int id, PageFilterRequest filter, IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.LogsByDeviceIdAsync(id, filter, cancellationToken);
    
    private static Task<PagedListResponse<DeviceLogsResponse>> LogsAsync(PageFilterRequest filter, IDevicesService devicesService, CancellationToken cancellationToken)
        => devicesService.LogsAsync(filter, cancellationToken);
}