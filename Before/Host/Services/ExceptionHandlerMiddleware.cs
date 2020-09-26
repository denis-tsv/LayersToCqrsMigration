using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Services;

namespace Host.Services
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (EntityNotFoundException ex)
            {
                await HandleAsync(httpContext, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                //Unauthorized http code equal to CheckOrderAsyncActionFilter
                await HandleAsync(httpContext, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (AggregateException ex)
            {
                var innerException = ex.InnerExceptions.First();
                if (innerException is EntityNotFoundException)
                    await HandleAsync(httpContext, HttpStatusCode.NotFound, innerException.Message);
                else if (innerException is ForbiddenException)
                    await HandleAsync(httpContext, HttpStatusCode.Unauthorized, innerException.Message);
                else
                    throw;
            }
        }

        private Task HandleAsync(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(message);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
