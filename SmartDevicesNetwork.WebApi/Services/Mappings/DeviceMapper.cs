using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Services.Mappings;

[Mapper]
public static partial class DeviceMapper
{
    [MapProperty(nameof(DevicesDtoModel.DeviceId), nameof(DevicesResponse.Id))]
    private static partial DevicesResponse MapToDeviceResponse(this DevicesDtoModel entity);
    
    public static partial IEnumerable<DevicesResponse> MapToListResponse(this IEnumerable<DevicesDtoModel> entities);
    
    [MapProperty(nameof(DeviceDtoModel.DeviceId), nameof(DeviceResponse.Id))]
    public static partial DeviceResponse MapToResponse(this DeviceDtoModel entity);
    
    [MapProperty(nameof(DeviceDtoModel.DeviceId), nameof(NetworkNodeResponse.Id))]
    private static partial NetworkNodeResponse MapToNetworkNodeResponse(this DevicesDtoModel entity);
    
    public static partial IEnumerable<NetworkNodeResponse> MapToResponse(this IEnumerable<DevicesDtoModel> entities);
    
    public static partial IEnumerable<DeviceLogsResponse> MapToResponse(this IEnumerable<DevicesLogsDto> entities);
}