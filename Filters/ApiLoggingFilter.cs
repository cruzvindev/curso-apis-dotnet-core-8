using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //Executa antes do método Action
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("################################################");
        _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("################################################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //Executa depois do método Action
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("################################################");
        _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
        _logger.LogInformation($"Status Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("################################################");
    }
}