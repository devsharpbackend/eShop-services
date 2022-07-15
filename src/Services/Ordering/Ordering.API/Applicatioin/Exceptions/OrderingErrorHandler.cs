namespace eShop.Services.Ordering.OrderingAPI.Application.Exceptions;


public class OrderingErrorHandler
{
    private readonly ILogger<HttpGlobalExceptionFilter> logger;
    private readonly IErrorHandler _errorHandler;
    private readonly IWebHostEnvironment _env;
    public OrderingErrorHandler(IErrorHandler errorHandler, IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        _errorHandler = errorHandler;
        this.logger = logger;
        _env = env;
    }
    public  JsonErrorResponse GetError(Exception Exception)
    {
        JsonErrorResponse jsonErrorResponse = new JsonErrorResponse();
        if(_env.IsDevelopment())
        {
            jsonErrorResponse.DeveloperMessage =Exception.ToString();
        }

        if (Exception.GetType() == typeof(NotFoundException))
        {
            jsonErrorResponse.Messages = new string[] { Exception.Message };
            jsonErrorResponse.StatusCode = (int)StatusCode.NotFound;
            
            return jsonErrorResponse;
        }

        if (Exception?.GetType() == typeof(ValidationException))
        {
            var validationException = Exception as ValidationException;
            var problemDetails = new ValidationProblemDetails()
            {
                //Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };

            foreach (var error in validationException.Errors)
            {
                problemDetails.Errors.Add(error.PropertyName, new[] { error.ErrorMessage });
            }

            jsonErrorResponse.Messages = problemDetails;
           
            jsonErrorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
           
            return jsonErrorResponse;
        }

        if (Exception.GetType() == typeof(OrderingDomainException))
        {
            var problemDetails = new ValidationProblemDetails()
            {
               
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };
            problemDetails.Errors.Add("DomainValidations", new string[] { Exception.Message.ToString() });
            jsonErrorResponse.Messages = problemDetails;
            jsonErrorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
           
            return jsonErrorResponse;
        }

        return  _errorHandler.GetError(Exception); ;
    }
}
