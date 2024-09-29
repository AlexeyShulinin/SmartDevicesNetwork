using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class NetworkEndpoint
{
    public static void RegisterNetwrorkEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/")
            .WithOpenApi();

        api.MapGet("network", NetworkAsync)
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get current network",
                Description = "Retrieve the network topology (list of devices and their connections)."
            })
            .Produces<NetworkResponse>();
    }

    private static Task<NetworkResponse> NetworkAsync(INetworkService networkService, CancellationToken cancellationToken)
        => networkService.ListAsync(cancellationToken);
}