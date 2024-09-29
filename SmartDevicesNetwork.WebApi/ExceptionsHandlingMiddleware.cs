using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Exceptions;
using SmartDevicesNetwork.WebApi.Resources;

namespace SmartDevicesNetwork.WebApi;

public class ExceptionsHandlingMiddleware(RequestDelegate next, IStringLocalizer<ApiMessages> apiMessagesLocalizer)
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
            apiMessagesLocalizer[ApiMessages.UnhandledServerErrorMessage]);

        if (ex is SdnBaseException exception)
        {
            exceptionResponse = new ExceptionResponse(exception.StatusCode, exception.Message);
        }
        
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(exceptionResponse);
    }

    private record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}