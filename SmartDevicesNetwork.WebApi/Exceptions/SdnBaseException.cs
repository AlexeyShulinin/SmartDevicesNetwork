using System;
using System.Net;

namespace SmartDevicesNetwork.WebApi.Exceptions;

public class SdnBaseException : Exception
{
    public SdnBaseException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public HttpStatusCode StatusCode { get; set; }
}