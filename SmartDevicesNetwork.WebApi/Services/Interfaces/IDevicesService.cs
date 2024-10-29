using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;

namespace SmartDevicesNetwork.WebApi.Services.Interfaces;

public interface IDevicesService
{
    Task<List<DevicesResponse>> DevicesListAsync(CancellationToken cancellationToken);
    
    Task<DeviceResponse> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken);
    
    Task<PagedListResponse<DeviceLogsResponse>> LogsByDeviceIdAsync(int deviceId, PageFilterRequest filter, CancellationToken cancellationToken);
    
    Task<PagedListResponse<DeviceLogsResponse>> LogsAsync(PageFilterRequest filter, CancellationToken cancellationToken);
}