using System.Linq;
using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Mappings;

[Mapper]
public static partial class NetworkMapper
{
    public static partial IQueryable<NetworkLinkDtoModel> ProjectToDto(this IQueryable<NetworkLink> entities);
}