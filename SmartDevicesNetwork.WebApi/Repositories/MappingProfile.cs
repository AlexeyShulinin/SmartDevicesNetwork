using AutoMapper;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Device, DeviceBaseDtoModel>();
        CreateMap<Device, DevicesDtoModel>()
            .IncludeBase<Device, DeviceBaseDtoModel>();
        CreateMap<Device, DeviceDtoModel>()
            .IncludeBase<Device, DeviceBaseDtoModel>();

        CreateMap<NetworkLink, NetworkLinkDtoModel>();

        CreateMap<Action, ActionsDtoModel>();
    }
}