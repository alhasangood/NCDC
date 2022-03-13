using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Portal.ViewModels;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var modelNotValid = context.HttpContext.Items.Any(i => i.Key.Equals("ValidationError"));
            if (modelNotValid)
            {
                var validationError = context.HttpContext.Items.Single(i => i.Key.Equals("ValidationError"));
                context.Result = new BadRequestObjectResult(validationError.Value);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
