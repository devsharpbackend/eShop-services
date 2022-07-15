namespace eShop.Services.Ordering.OrderingAPI.Infrastructure.Interceptors;

public class LogServerInterceptor : Interceptor
{
    private readonly ILogger<LogServerInterceptor> _logger;

    public LogServerInterceptor(ILogger<LogServerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Incomming Request Method: {context.Method}.");
        return await continuation(request, context);

    }
}

