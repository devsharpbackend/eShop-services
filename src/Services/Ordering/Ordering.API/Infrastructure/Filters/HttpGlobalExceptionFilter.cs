namespace eShop.Services.Ordering.OrderingAPI.Infrastructure.Filters;
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

        if (context.Exception.GetType() == typeof(OrderingApplicationException))
        {
            var catalogApplicationException = context.Exception as OrderingApplicationException;
            context.Result = new ObjectResult(catalogApplicationException.JsonErrorResponse);
            context.HttpContext.Response.StatusCode = catalogApplicationException.JsonErrorResponse.StatusCode;
            return;
        }

        var res = _errorHandler.GetError(context.Exception);
        context.Result = new ObjectResult(res);
        context.HttpContext.Response.StatusCode = res.StatusCode;


    }
}
