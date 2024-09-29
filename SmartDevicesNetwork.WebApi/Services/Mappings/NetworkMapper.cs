using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Services.Mappings;

[Mapper]
public static partial class NetworkMapper
{
    [MapProperty(nameof(NetworkLinkDtoModel.SourceId), nameof(NetworkLinkResponse.Source))]
    [MapProperty(nameof(NetworkLinkDtoModel.TargetId), nameof(NetworkLinkResponse.Target))]
    [MapProperty(nameof(NetworkLinkDtoModel.LinkType), nameof(NetworkLinkResponse.Type))]
    public static partial NetworkLinkResponse MapToResponse(this NetworkLinkDtoModel entity);
    public static partial IEnumerable<NetworkLinkResponse> MapToResponse(this IEnumerable<NetworkLinkDtoModel> entities);
}