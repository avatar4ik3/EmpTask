using Managment.Common.Models.Dtos.Responses;
using Microsoft.AspNetCore.Http;

namespace Managment.Common.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BadHttpRequestException exception)
        {
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Error = exception.Message });
        }
        catch
        {
            throw;
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}