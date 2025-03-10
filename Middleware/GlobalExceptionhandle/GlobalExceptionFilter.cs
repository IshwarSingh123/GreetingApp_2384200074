using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Middleware.GlobalExceptionHandler
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var errorResponse = ExceptionHandler.HandleException(context.Exception, _logger);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500 // Internal server error
            };

            context.ExceptionHandled = true;
        }
    }
}
