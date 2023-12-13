using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Service.Exceptions;

namespace MyCarForSale.API.Middlewares;

public static class UseCustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var statusCode = exceptionFeature.Error switch
                {
                    ClientSideException => 400,
                    UnauthorizedException => 401,
                    ForbiddenException => 403,
                    NotFoundException => 404,
                    _ => 500
                };

                context.Response.StatusCode = statusCode;
                var response = CustomNoContentResponseDto.Fail(statusCode, exceptionFeature.Error.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });
    }
}