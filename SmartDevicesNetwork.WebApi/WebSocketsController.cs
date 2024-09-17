using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi;

public static class WebSocketsController
{
    public static void RegisterWebSocketsHandler(this WebApplication app)
    {
        app.MapGet("/ws", async (INetworkService networkService, HttpContext context, CancellationToken cancellationToken) =>
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                throw new SdnBaseException(HttpStatusCode.BadRequest, "Not a web sockets request");
            }
            
            using var ws = await context.WebSockets.AcceptWebSocketAsync();
            while (true)
            {
                var network = await networkService.ListAsync(cancellationToken);
                await ws.SendAsync(
                    JsonSerializer.SerializeToUtf8Bytes(network),
                    WebSocketMessageType.Binary,
                    WebSocketMessageFlags.EndOfMessage,
                    cancellationToken);
                await Task.Delay(5000, cancellationToken);
            }
        });
    }
}