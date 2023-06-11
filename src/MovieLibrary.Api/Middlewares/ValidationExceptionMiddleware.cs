using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieLibrary.Api.Models;

namespace MovieLibrary.Api.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new ErrorResponse
        {
            Message = "Validation Failed",
            Code = "validation_failed",
            Errors = exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(e => e.Key, e => e.Select(v => v.ErrorMessage).ToArray())
        };

        var result = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(result);
    }
}
