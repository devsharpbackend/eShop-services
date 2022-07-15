namespace eShop.Services.CatalogAPI.Infrastructure.Interceptors;

public class GrpcClientExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcClientExceptionInterceptor> _logger;

    public GrpcClientExceptionInterceptor(ILogger<GrpcClientExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Starting call. Type: {context.Method.Type}. " +
             $"Method: {context.Method.Name}.");
        var call = continuation(request, context);

        return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
    }

    private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> task)
    {
        try
        {
            var response = await task;
            return response;
        }
        catch (RpcException e)
        {

            _logger.LogError("Error calling via grpc: {Status} - {Message}", e.Status, e.Message);
            return default;
        }
    }
}
