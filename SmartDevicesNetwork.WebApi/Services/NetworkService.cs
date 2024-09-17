using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Services;

public class NetworkService(IUnitOfWork unitOfWork, IMapper mapper) : INetworkService
{
    public async Task<NetworkResponse> ListAsync(CancellationToken cancellationToken)
        => new(mapper.Map<List<DevicesDtoModel>, List<NetworkNodeResponse>>(
                await unitOfWork.DevicesRepository.ListAsync(cancellationToken)),
            mapper.Map<List<NetworkLinkDtoModel>, List<NetworkLinkResponse>>(
                await unitOfWork.NetworkRepository.ListAsync(cancellationToken)));
}