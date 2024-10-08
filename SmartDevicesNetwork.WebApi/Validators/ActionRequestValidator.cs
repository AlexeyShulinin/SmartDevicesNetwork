using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Enums;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Resources;

namespace SmartDevicesNetwork.WebApi.Validators;

public class ActionRequestValidator : AbstractValidator<ActionRequest>
{
    public ActionRequestValidator(IStringLocalizer<ApiMessages> apiMessagesLocalizer)
    {
        RuleFor(x => x.Action)
            .Must(a => Enum.IsDefined(typeof(Actions), a))
            .WithMessage(x => string.Format(apiMessagesLocalizer[ApiMessages.ActionNotFoundErrorMessage], x.Action));
    }
}