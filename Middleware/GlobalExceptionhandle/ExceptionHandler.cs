using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Middleware.GlobalExceptionHandler
{

    public class ExceptionHandler
    {
        public static object HandleException(Exception ex, ILogger logger)
        {
            logger.LogError(ex, "An unexpected error occurred.");

            return new
            {
                Success = false,
                Message = "An unexpected error occurred.",
                ErrorDetails = ex.Message
            };
        }

    }
}

