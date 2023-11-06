using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCrm.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        // TODO: you may want to inject an ILogger and write messages in the OnException method below.
        private ILogger<GlobalExceptionFilter> _logger { get; set; }

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void Dispose() { }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            // Check for ApiException in context.Exception,
            // - if present use its status code and model in the object result below
            // - if another type, pick a default status code and anything you prefer for the model
            var statusCode = exception is ApiException ? ((ApiException)exception).StatusCode : 400;
            var item = exception is ApiException ? ((ApiException)exception).Model : context.RouteData.Values;
            var message = exception is ApiException ? ((ApiException)exception).Message : "An error occurred.";

            var model = new
            {
                success = false,
                messages = new string[] { message },
                item = item
            };

            var eventId = new EventId(statusCode);
            _logger.LogError(eventId, exception, "An error occured, was model state valid: {0}, Exception details: {1}", context.ModelState.IsValid, exception.StackTrace);

            // no return type, instead you set the context.Result as last line in this method
            // an object with the error details
            context.Result = new ObjectResult(model) { StatusCode = statusCode };
        }
    }
}
