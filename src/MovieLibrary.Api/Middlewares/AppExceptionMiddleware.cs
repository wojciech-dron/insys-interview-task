using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MovieLibrary.Api.Models;
using MovieLibrary.Core.Exceptions;

public class AppExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public AppExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ErrorResponse();

        if (exception is AppException appException)
        {
            response.Message = appException.Message;
            response.Code = appException.AppCode;
            context.Response.StatusCode = (int)appException.StatusCode;
            
            if (_environment.IsDevelopment())
                response.StackTrace = appException.StackTrace;
        }
        else
            response.Message = exception.Message;

        var result = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(result);
    }
}