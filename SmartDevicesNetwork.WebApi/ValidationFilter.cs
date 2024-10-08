using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Resources;

namespace SmartDevicesNetwork.WebApi;

public class ValidationFilter<T>(IStringLocalizer<ApiMessages> apiMessagesLocalizer) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            return await next(context);
        }
        
        var entity = context.Arguments
            .OfType<T>()
            .FirstOrDefault(a => a.GetType() == typeof(T));
        if (entity is not null)
        {
            var results = await validator.ValidateAsync(entity);
            if (!results.IsValid)
            {
                return Results.ValidationProblem(results.ToDictionary());
            }

            return await next(context);
        }

        return Results.Problem(apiMessagesLocalizer[ApiMessages.ValidationObjectNotFound]);
    }
}