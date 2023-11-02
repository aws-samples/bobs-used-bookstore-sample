using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Bookstore.Web.Helpers
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> logger;

        public LogActionFilter(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<LogActionFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogDebug("Action method {action} completed execution at {time}", context.ActionDescriptor.DisplayName, DateTime.Now.ToString());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogDebug("Action method {action} invoked at {time}", context.ActionDescriptor.DisplayName, DateTime.Now.ToString());
        }
    }
}
