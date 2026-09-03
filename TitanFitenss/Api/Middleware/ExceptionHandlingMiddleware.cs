using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Common.Exceptions;
using ValidationException=TitanFitenss.Application.Common.Exceptions.ValidationException;
namespace TitanFitenss.Api.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next=next;
        _logger=logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType="application/problem+json";
        object responseBody;
        int statusCode;
        switch (exception)
        {
            case ValidationException validationException:
                statusCode=(int)HttpStatusCode.BadRequest;
                responseBody=new ValidationProblemDetails(validationException.Errors)
                {
                    Status=statusCode,
                    Title="One or more validation errors occurred."
                };
                break;

            case NotFoundException notFoundException:
                statusCode=(int)HttpStatusCode.NotFound;
                responseBody=new ProblemDetails
                {
                    Status=statusCode,
                    Title="Resource not found.",
                    Detail=notFoundException.Message
                };
                break;

            case BusinessRuleException or ArgumentException or InvalidOperationException:
                statusCode=(int)HttpStatusCode.BadRequest;
                responseBody=new ProblemDetails
                {
                    Status=statusCode,
                    Title="The request could not be completed.",
                    Detail=exception.Message
                };
                break;

            default:
                _logger.LogError(exception, "Unhandled exception");
                statusCode=(int)HttpStatusCode.InternalServerError;
                responseBody=new ProblemDetails
                {
                    Status=statusCode,
                    Title="An unexpected error occurred.",
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
    }
}
