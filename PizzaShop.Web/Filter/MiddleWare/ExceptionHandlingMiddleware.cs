using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Continue request pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Ensure response is not already started
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("The response has already started, cannot modify the error response.");
            return;
        }

        var factory = context.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
        var tempData = factory?.GetTempData(context);

        if (tempData != null)
        {
            tempData["ErrorMessage"] = exception.Message; // Store exception message
            tempData["StackTrace"] = exception.StackTrace ?? "No stack trace available.";
            tempData["RequestId"] = Activity.Current?.Id ?? context.TraceIdentifier;
        }

        _logger.LogError("Exception handled: {Message}", exception.Message);

        // Redirect to Error Page
        await Task.CompletedTask; // Ensure task completion
    }
}
