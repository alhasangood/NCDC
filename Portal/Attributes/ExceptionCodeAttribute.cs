using Microsoft.AspNetCore.Mvc.Filters;

namespace Portal.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExceptionCodeAttribute : Attribute, IActionFilter
{
    private readonly string code;

    public ExceptionCodeAttribute(string code)
    {
        this.code = code;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        return;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items.Add("EX", code);
    }
}
