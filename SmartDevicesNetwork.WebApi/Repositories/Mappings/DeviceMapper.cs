using System.Linq;
using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Mappings;

[Mapper]
public static partial class DeviceMapper
{
    public static partial IQueryable<DevicesDtoModel> ProjectToListDto(this IQueryable<Device> entities);
    public static partial IQueryable<DeviceDtoModel> ProjectToDto(this IQueryable<Device> entities);
    public static partial Device MapToEntity(this DeviceDtoModel model);
}