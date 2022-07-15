
namespace eShop.Services.CatalogAPI.Infrastructure.Interceptors;



public class GrpcServerExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcServerExceptionInterceptor> _logger;
    private readonly IErrorHandler _errorHandler;
    public GrpcServerExceptionInterceptor(ILogger<GrpcServerExceptionInterceptor> logger,
        IErrorHandler errorHandler
        )
    {
        _logger = logger;
        _errorHandler = errorHandler;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }

        catch (Exception exception)
        {
            var httpContext = context.GetHttpContext();


            if (exception.GetType() == typeof(ValidationException))
            {
                var validationException = exception as ValidationException;
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = httpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                foreach (var error in validationException.Errors)
                {
                    problemDetails.Errors.Add(error.PropertyName, new[] { error.ErrorMessage });
                }
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                throw new RpcException(new Status(StatusCode.Internal, JsonSerializer.Serialize<ValidationProblemDetails>(problemDetails)));
            }




            var res = _errorHandler.GetError(exception);

        
            httpContext.Response.StatusCode = res.StatusCode;
            throw new RpcException(new Status(StatusCode.Internal, JsonSerializer.Serialize(res)));



           
        }
    }
}