namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure;

public class GrpcExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcExceptionInterceptor> _logger;
    private readonly ITokenProvider _tokenProvider;
    public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _tokenProvider = tokenProvider;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        
        var call = continuation(request, context);
        var token =  _tokenProvider.GetTokenAsync().Result;
    
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
            if (e.Status.ToString().Contains("401"))
            {
                throw new UnauthorizedAccessException();
            }
            return default;
        }
    }
}
