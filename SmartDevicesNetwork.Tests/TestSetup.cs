using Imposter.Abstractions;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;

[assembly: GenerateImposter(typeof(IUnitOfWork))]

[assembly: GenerateImposter(typeof(IDevicesRepository))]

[assembly: GenerateImposter(typeof(IActionsRepository))]

[assembly: GenerateImposter(typeof(INetworkRepository))]