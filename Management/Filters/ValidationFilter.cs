using Management.ViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //before controller
            if (!context.ModelState.IsValid)
            {

                var modelStateError = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new ErrorModel() {
                        Message = x.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault(),
                        StatusCode = x.Key
                    }).SingleOrDefault();
                context.Result = new BadRequestObjectResult(modelStateError);
                return;
            }
            //after controller
            await next();

        }
    }

    
}
