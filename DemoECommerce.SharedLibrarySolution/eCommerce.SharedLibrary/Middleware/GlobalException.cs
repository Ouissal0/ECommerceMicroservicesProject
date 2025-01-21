using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

public class GlobalException
{
    private readonly RequestDelegate _next;

    public GlobalException(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Déclaration par défaut
        string message = "Sorry, Internal server error occurred. Kindly try again.";
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string title = "Error";

        try
        {
            await _next(context);

            // Gestion des codes spécifiques
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Warning";
                message = "Too many requests made.";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "Alert";
                message = "You are not authorized to access.";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Out of Access";
                message = "You are not allowed to access.";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
            }
        }
        catch (Exception ex)
        {
            // Log de l'exception
            LogException.LogExceptions(ex);

            // Si l'exception est un timeout
            if (ex is TaskCanceledException || ex is TimeoutException)
            {
                title = "Out of time";
                message = "Request timeout. Please try again.";
                statusCode = StatusCodes.Status408RequestTimeout;
            }

            // Réponse par défaut pour les exceptions non gérées
            await ModifyHeader(context, title, message, statusCode);
        }
    }

    private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Title = title,
            Detail = message,
            Status = statusCode
        }));
    }
}
