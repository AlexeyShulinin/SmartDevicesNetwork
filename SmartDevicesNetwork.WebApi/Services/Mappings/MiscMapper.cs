using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Models.Responses;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Services.Mappings;

[Mapper]
public static partial class MiscMapper
{
    public static partial BaseEntityResponse MapToBaseEntityResponse(this BaseEntityDto dtoModel);
    
    public static partial PageFilterDto MapToDto(this PageFilterRequest model); 
}