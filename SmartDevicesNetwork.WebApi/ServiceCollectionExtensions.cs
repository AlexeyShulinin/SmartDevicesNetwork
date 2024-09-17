using Microsoft.Extensions.DependencyInjection;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi;

public static class ServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IActionsService, ActionsService>();
        serviceCollection.AddScoped<IDevicesService, DevicesService>();
        serviceCollection.AddScoped<INetworkService, NetworkService>();
    }
}