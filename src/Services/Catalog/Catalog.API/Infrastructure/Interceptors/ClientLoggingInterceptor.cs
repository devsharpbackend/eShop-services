
namespace eShop.Services.CatalogAPI.Infrastructure.Interceptors;

public class ClientLoggingInterceptor : Interceptor
{
    private readonly ILogger<ClientLoggingInterceptor> _logger;
    public ClientLoggingInterceptor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ClientLoggingInterceptor>();
    }
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Starting call. Type: {context.Method.Type}. " +
            $"Method: {context.Method.Name}.");



        var call = continuation(request, context);



        async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
        {
            var response = await inner;


            _logger.LogInformation($"finish call. Type: {context.Method.Type}. " +
            $"Method: {context.Method.Name}.");

            return response;

        }

        return new AsyncUnaryCall<TResponse>(
          HandleResponse(call.ResponseAsync)
          ,
          call.ResponseHeadersAsync,
          call.GetStatus,
          call.GetTrailers,
          call.Dispose);
    }

    //private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
    //{
    //    var response = await inner;


    //    _logger.LogInformation($"finish call. Type");

    //    return response;

    //}
}


