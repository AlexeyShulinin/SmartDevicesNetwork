
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services.Interfaces;
using SmartDevicesNetwork.WebApi.Services.Mappings;

namespace SmartDevicesNetwork.WebApi.Services;

public class NetworkService(IUnitOfWork unitOfWork) : INetworkService
{
    public async Task<NetworkResponse> ListAsync(CancellationToken cancellationToken)
        => new((await unitOfWork.DevicesRepository.ListAsync(cancellationToken)).MapToResponse(),
            (await unitOfWork.NetworkRepository.ListAsync(cancellationToken)).MapToResponse());
}