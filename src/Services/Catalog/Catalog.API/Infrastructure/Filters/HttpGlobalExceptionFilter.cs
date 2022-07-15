


namespace eShop.Services.Catalog.Catalog.API.Infrastructure.Filters;
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

        if (context.Exception.GetType() == typeof(NotFoundException))
        {
            context.Result = new ObjectResult(context.Exception.Message);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        if (context.Exception?.GetType() == typeof(ValidationException))
        {
            var validationException = context.Exception as ValidationException;
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            foreach (var error in validationException.Errors)
            {
                problemDetails.Errors.Add(error.PropertyName, new[] { error.ErrorMessage });
            }
            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }


        if (context.Exception.GetType() == typeof(CatalogDomainException))
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });
            
            
            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }




        var res = _errorHandler.GetError(context.Exception);
        context.Result = new ObjectResult(res);
        context.HttpContext.Response.StatusCode = res.StatusCode;

    }
}
