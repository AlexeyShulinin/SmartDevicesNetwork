using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Services.Interfaces;

namespace SmartDevicesNetwork.WebApi.Services;

public class DevicesService(IUnitOfWork unitOfWork, IMapper mapper) : IDevicesService
{
    public async Task<List<DevicesResponse>> DevicesListAsync(CancellationToken cancellationToken)
        => mapper.Map<List<DevicesDtoModel>, List<DevicesResponse>>(await unitOfWork.DevicesRepository.ListAsync(cancellationToken));

    public async Task<DeviceResponse> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken)
    {
        var device = mapper.Map<DeviceDtoModel, DeviceResponse>(await unitOfWork.DevicesRepository.ByIdAsync(deviceId, cancellationToken));
        if (device == null)
        {
            throw new NotFoundException();
        }

        return device;
    }
}