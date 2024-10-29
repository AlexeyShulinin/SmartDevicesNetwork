using System.Linq;
using Riok.Mapperly.Abstractions;
using SmartDevicesNetwork.WebApi.Database.Models;
using SmartDevicesNetwork.WebApi.Repositories.Models;

namespace SmartDevicesNetwork.WebApi.Repositories.Mappings;

[Mapper]
public static partial class ActionMapper
{
    public static partial IQueryable<ActionsDtoModel> ProjectToDto(this IQueryable<Action> entities);
    
    public static partial Action MapToEntity(this ActionsDtoModel model);
}