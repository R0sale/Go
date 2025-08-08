using Entities.ExceptionModels;
using Microsoft.AspNetCore.Diagnostics;
using Entities.Exceptions;

namespace Web.Extensions
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this WebApplication app, Serilog.ILogger logger)
        {
            app.UseExceptionHandler(appError => 
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            ArgumentNullException => StatusCodes.Status400BadRequest,
                            ArgumentException => StatusCodes.Status400BadRequest,
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.Error(contextFeature.Error, contextFeature.Error.Message);

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                }));
        }
    }
}
