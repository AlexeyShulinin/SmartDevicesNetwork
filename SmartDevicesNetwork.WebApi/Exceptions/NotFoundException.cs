using System.Net;

namespace SmartDevicesNetwork.WebApi.Exceptions;

public class NotFoundException : SdnBaseException
{
    public NotFoundException() : base(HttpStatusCode.NotFound, "Entity not found") { }
}