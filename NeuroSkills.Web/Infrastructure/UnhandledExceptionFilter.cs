using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NeuroSkills.Web.Models;
using System.Net;
using System.Net.Sockets;

namespace NeuroSkills.Web.Infrastructure;

public class UnhandledExceptionFilter : IExceptionFilter
{
    private readonly ILogger<UnhandledExceptionFilter> logger;

    public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "Se produjo un error");

        var (status, data) = GetDetailFromException(context.Exception);

        if (context.HttpContext.Request.IsAjaxRequest())
            context.Result = new JsonResult(data);
        else
        {
            var model = new ErrorViewModel
            {
                RequestId = context.HttpContext.TraceIdentifier
            };

            var vdd = new ViewDataDictionary<ErrorViewModel>(
                    new EmptyModelMetadataProvider(),
                    context.ModelState)
            { Model = model };

            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = vdd
            };
        }
    }

    private (HttpStatusCode status, object data) GetDetailFromException(Exception e)
    {
        var message = "Se produjo un error inesperado. Por favor contacta al administrador si el problema persiste.";
        var inner = e;

        while (inner.InnerException != null)
        {
            inner = inner.InnerException;
        }

        var statusCode = e switch
        {
            ArgumentException _ => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        if (inner is SocketException se && se.SocketErrorCode == SocketError.TimedOut)
            statusCode = HttpStatusCode.RequestTimeout;
        else if (inner is TimeoutException)
            statusCode = HttpStatusCode.RequestTimeout;

        return (statusCode, new { Success = false, Messages = new[] { message } });
    }
}

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (request.Headers != null)
            return request.Headers["X-Requested-With"] == "XMLHttpRequest" || request.Headers.ContainsKey("origin") || request.ContentType == "application/json";

        return false;
    }
}