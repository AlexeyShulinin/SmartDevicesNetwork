using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services.Interfaces;
using SmartDevicesNetwork.WebApi.Services.Mappings;

namespace SmartDevicesNetwork.WebApi.Services;

public class DevicesService(IUnitOfWork unitOfWork) : IDevicesService
{
    public async Task<List<DevicesResponse>> DevicesListAsync(CancellationToken cancellationToken)
        => (await unitOfWork.DevicesRepository.ListAsync(cancellationToken)).MapToListResponse().ToList();

    public async Task<DeviceResponse> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken)
    {
        var device = (await unitOfWork.DevicesRepository.ByIdAsync(deviceId, cancellationToken)).MapToResponse();
        if (device == null)
        {
            throw new NotFoundException();
        }

        return device;
    }
}