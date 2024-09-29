using Microsoft.AspNetCore.Builder;

namespace SmartDevicesNetwork.WebApi.Endpoints;

public static class RegisterEndpoint
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.RegisterDevicesEndpoints();
        app.RegisterNetwrorkEndpoints();
        app.RegisterWebSocketsEndpoints();
    }
}