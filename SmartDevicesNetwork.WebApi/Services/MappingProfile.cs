using AutoMapper;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DevicesDtoModel, DevicesResponse>()
            .ForCtorParam(nameof(DevicesResponse.Id), opt => opt.MapFrom(x => x.DeviceId));
        CreateMap<DeviceDtoModel, DeviceResponse>()
            .ForCtorParam(nameof(DevicesResponse.Id), opt => opt.MapFrom(x => x.DeviceId))
            .ForCtorParam(nameof(DeviceResponse.Details), 
                opt => opt.MapFrom(x =>
                    new DeviceDetailsResponse(x.IpAddress, x.FirmwareVersion, x.BatteryLevel)));

        CreateMap<DevicesDtoModel, NetworkNodeResponse>()
            .ForCtorParam(nameof(DevicesResponse.Id), opt => opt.MapFrom(x => x.DeviceId));
        CreateMap<NetworkLinkDtoModel, NetworkLinkResponse>()
            .ForCtorParam(nameof(NetworkLinkResponse.Source), opt => opt.MapFrom(x => x.SourceId))
            .ForCtorParam(nameof(NetworkLinkResponse.Target), opt => opt.MapFrom(x => x.TargetId))
            .ForCtorParam(nameof(NetworkLinkResponse.Type), opt => opt.MapFrom(x => x.LinkType));
    }
}