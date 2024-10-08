using System;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Resources;
using SmartDevicesNetwork.WebApi.Services.Interfaces;
using WebSocketOptions = SmartDevicesNetwork.WebApi.Options.WebSocketOptions;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class WebSocketsEndpoint
{
    public static void RegisterWebSocketsEndpoints(this WebApplication app)
    {
        app.MapGet("/ws", NetworkAsync);
    }
    
    private static async Task NetworkAsync(
        INetworkService networkService,
        HttpContext context,
        IStringLocalizer<ApiMessages> apiMessagesLocalizer,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            throw new SdnBaseException(HttpStatusCode.BadRequest, apiMessagesLocalizer[ApiMessages.NotWebSocketRequest]);
        }

        var webSocketOptions = new WebSocketOptions();
        configuration.GetSection("WebSocket").Bind(webSocketOptions);
        
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(webSocketOptions.TimeSeconds));
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
            }
        }
        catch (OperationCanceledException e)
        {
            throw new SdnBaseException(HttpStatusCode.OK, e.Message);
        }
    }
}