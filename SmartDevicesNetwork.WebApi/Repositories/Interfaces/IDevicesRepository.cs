using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Interfaces;

public interface IDevicesRepository
{
    Task<List<DevicesDtoModel>> ListAsync(CancellationToken cancellationToken);
    
    Task<DeviceDtoModel> ByIdAsync(int deviceId, CancellationToken cancellationToken);
    
    void Update(DeviceDtoModel device, string message);
    
    Task<PagedListDto<DevicesLogsDto>> LogsListByDeviceIdAsync(int deviceId, PageFilterDto filter, CancellationToken cancellationToken);
    Task<PagedListDto<DevicesLogsDto>> LogsListAsync(PageFilterDto filter, CancellationToken cancellationToken);
}