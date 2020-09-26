using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Host.Services
{
    public class CheckOrderAsyncActionFilter : ActionFilterAttribute
    {
        //dbContext and currentUserService can be injected to constructor but it will require to add ServiceFilter attribute to controller methods. 
        //I'd like to add [CheckOrderAsyncActionFilter] instead of [ServiceFilter(typeof(CheckOrderAsyncActionFilter))] for controller methods
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<IDbContext>();

            var id = (int)context.ActionArguments["id"];
            var order = await dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var currentUserService = context.HttpContext.RequestServices.GetService<ICurrentUserService>();
            if (order.UserEmail != currentUserService.Email)
            {
                context.Result = new UnauthorizedResult(); //ForbidResult requires authentication so we will use Unauthorized instead of it
                return;
            }

            await next();
        }
    }
}
