using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;
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

    public async Task<PagedListResponse<DeviceLogsResponse>> LogsByDeviceIdAsync(int deviceId, PageFilterRequest filter, CancellationToken cancellationToken)
    {
        var device = (await unitOfWork.DevicesRepository.ByIdAsync(deviceId, cancellationToken)).MapToResponse();
        if (device == null)
        {
            throw new NotFoundException();
        }
        
        return MapToPagedListResponse(await unitOfWork.DevicesRepository.LogsListByDeviceIdAsync(deviceId, filter.MapToDto(), cancellationToken), filter);
    }

    public async Task<PagedListResponse<DeviceLogsResponse>> LogsAsync(PageFilterRequest filter, CancellationToken cancellationToken)
        => MapToPagedListResponse(await unitOfWork.DevicesRepository.LogsListAsync(filter.MapToDto(), cancellationToken), filter);

    private PagedListResponse<DeviceLogsResponse> MapToPagedListResponse(
        PagedListDto<DevicesLogsDto> devicePagedListDto, PageFilterRequest filter)
        => new(devicePagedListDto.Items.MapToResponse(), 
            devicePagedListDto.CurrentPage,
            devicePagedListDto.Total,
            (filter.Page + 1) * filter.Limit < devicePagedListDto.Total ? filter.Page + 1 : null);
}