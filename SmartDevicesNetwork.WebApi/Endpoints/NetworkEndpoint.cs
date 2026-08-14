using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class NetworkEndpoint
{
    public static void RegisterNetworkEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/");

        api.MapGet("network", NetworkAsync)
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Summary = "Get current network";
                operation.Description = "Retrieve the network topology (list of devices and their connections).";
                return Task.CompletedTask;
            })
            .Produces<NetworkResponse>();
    }

    private static Task<NetworkResponse> NetworkAsync(INetworkService networkService, CancellationToken cancellationToken)
        => networkService.ListAsync(cancellationToken);
}
