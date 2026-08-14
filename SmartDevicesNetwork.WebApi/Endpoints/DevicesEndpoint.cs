using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class DevicesEndpoint
{
    public static void RegisterDevicesEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/");

        api.MapGet("devices", DevicesAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Get list of devices";
                operation.Description = "Retrieve a list of all devices.";
                return Task.CompletedTask;
            })
            .Produces<List<DevicesResponse>>();

        api.MapGet("devices/{id}", DeviceByIdAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Get device details";
                operation.Description = "Retrieve detailed information about a specific device by its ID.";
                return Task.CompletedTask;
            })
            .Produces<DeviceResponse>();

        api.MapPost("devices/{id}/action", PerformActionAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Perform action";
                operation.Description = "Perform an action on a device (turn on, reboot, etc.).";
                return Task.CompletedTask;
            })
            .Produces<ActionResponse>()
            .AddEndpointFilter<ValidationFilter<ActionRequest>>();

        api.MapPost("devices/{id}/logs", LogsByDeviceIdAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Get device logs";
                operation.Description = "Retrieve device logs.";
                return Task.CompletedTask;
            })
            .Produces<PagedListResponse<DeviceLogsResponse>>();

        api.MapPost("devices/logs", LogsAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Get all device logs";
                operation.Description = "Retrieve device logs.";
                return Task.CompletedTask;
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
