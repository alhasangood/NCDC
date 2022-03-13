using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Portal.ViewModels;

public class ValidatorInterceptor : IValidatorInterceptor
{
    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
    {
        var obj = commonContext.InstanceToValidate;

        foreach (var prop in obj.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
        {
            var value = prop.GetValue(obj) as string;

            value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();

            prop.SetValue(obj, value);
        }

        return commonContext;
    }

    public FluentValidation.Results.ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, FluentValidation.Results.ValidationResult result)
    {
        if (!result.IsValid)
        {
            actionContext.HttpContext.Items.Add("ValidationError", new
            {
                statusCode = result.Errors[0].ErrorCode,
                message = result.Errors[0].ErrorMessage
            });
        }

        return result;
    }
}
