using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi;

public static class Controller
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/")
            .WithOpenApi();
        
        api.MapGet("devices", (IDevicesService devicesService, CancellationToken cancellationToken) 
                => devicesService.DevicesListAsync(cancellationToken))
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get list of devices",
                Description = "Retrieve a list of all devices."
            })
            .Produces<List<DevicesResponse>>();
        
        api.MapGet("devices/{id}", (int id, IDevicesService devicesService, CancellationToken cancellationToken) 
                => devicesService.GetDeviceByIdAsync(id, cancellationToken))
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get device details",
                Description = "Retrieve detailed information about a specific device by its ID."
            })
            .Produces<DeviceResponse>();
        
        api.MapPost("devices/{id}/action", (int id, ActionRequest actionRequest, IActionsService actionService, CancellationToken cancellationToken) 
                => actionService.PerformActionAsync(id, actionRequest, cancellationToken))
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Perform action",
                Description = "Perform an action on a device (turn on, reboot, etc.)."
            })
            .Produces<ActionResponse>();

        api.MapGet("network", (INetworkService networkService, CancellationToken cancellationToken) 
                => networkService.ListAsync(cancellationToken))
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get current network",
                Description = "Retrieve the network topology (list of devices and their connections)."
            })
            .Produces<NetworkResponse>();
    }
}