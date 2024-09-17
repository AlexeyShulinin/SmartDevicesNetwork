using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartDevicesNetwork.WebApi.Exceptions;

namespace SmartDevicesNetwork.WebApi;

public class ExceptionsHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ExceptionResponse exceptionResponse = new ExceptionResponse(
            HttpStatusCode.InternalServerError, 
            "Unhandled internal server error");

        if (ex is SdnBaseException exception)
        {
            exceptionResponse = new ExceptionResponse(exception.StatusCode, exception.Message);
        }
        
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(exceptionResponse);
    }

    private record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}