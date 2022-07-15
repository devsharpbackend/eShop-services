
using eShop.BuildingBlocks.Common.ErrorHandler;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Filters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{

    private readonly ILogger<HttpGlobalExceptionFilter> logger;
    private readonly IErrorHandler _errorHandler;
    public HttpGlobalExceptionFilter(IErrorHandler errorHandler, ILogger<HttpGlobalExceptionFilter> logger)
    {
        _errorHandler = errorHandler;
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(new EventId(context.Exception.HResult),
             context.Exception,
             context.Exception.Message);

        context.ExceptionHandled = true;

       
        if (context.Exception.GetType() == typeof(UnauthorizedAccessException))
        {
           
            context.Result = new ObjectResult("no access");
            context.HttpContext.Response.StatusCode = 401;
            return;
        }
        var res = _errorHandler.GetError(context.Exception);
        context.Result = new ObjectResult(res);
        context.HttpContext.Response.StatusCode = res.StatusCode;

    }
}
