using System;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class WebSocketsEndpoint
{
    public static void RegisterWebSocketsEndpoints(this WebApplication app)
    {
        app.MapGet("/ws", NetworkAsync);
    }
    
    private static async Task NetworkAsync(INetworkService networkService, HttpContext context, CancellationToken cancellationToken)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            // TODO: Move to resources
            throw new SdnBaseException(HttpStatusCode.BadRequest, "Not a web sockets request");
        }
            
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                var network = await networkService.ListAsync(cancellationToken);
                await ws.SendAsync(
                    JsonSerializer.SerializeToUtf8Bytes(network, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }),
                    WebSocketMessageType.Binary,
                    WebSocketMessageFlags.EndOfMessage,
                    cancellationToken);
                await Task.Delay(5000, cancellationToken);
            }
        }
        catch (OperationCanceledException e)
        {
            throw new SdnBaseException(HttpStatusCode.OK, e.Message);
        }
    }
}