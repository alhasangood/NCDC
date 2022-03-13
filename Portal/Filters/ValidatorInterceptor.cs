using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Filters
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        

        IValidationContext IValidatorInterceptor.BeforeAspNetValidation(ActionContext context, IValidationContext validationContext)
        {
            return validationContext;
        }

        ValidationResult IValidatorInterceptor.AfterAspNetValidation(ActionContext context, IValidationContext validationContext, ValidationResult result)
        {

            var projection = result.Errors.Select(failure => new ValidationFailure(failure.PropertyName,failure.ErrorMessage) { 
                ErrorCode = failure.ErrorCode,
                ErrorMessage = failure.ErrorMessage
            }).FirstOrDefault();
            if (projection is not null)
                context.ModelState.AddModelError(projection.ErrorCode, projection.ErrorMessage);
            return new ValidationResult();
        }

    }
}
