using FluentValidation;
using SmartDevicesNetwork.WebApi.Models.Requests;

namespace SmartDevicesNetwork.WebApi.Validators;

public class PageFilterRequestValidator : AbstractValidator<PageFilterRequest>
{
    public PageFilterRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000);
    }
}