
namespace eShop.Services.Ordering.OrderingAPI.Application.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly OrderingErrorHandler _orderingErrorHandler;
    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger,
        OrderingErrorHandler orderingErrorHandler
        )
    {
        _logger = logger?? throw new  ArgumentNullException(nameof(logger));
        _orderingErrorHandler = orderingErrorHandler ?? throw new ArgumentNullException(nameof(_orderingErrorHandler));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName()
         , request);

            var response = await next();

            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), response);

            return response;
        }
       
        catch (Exception ex)
        {
            _logger.LogError("----- Error in  Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), ex.ToString());
            throw new OrderingApplicationException(ex, _orderingErrorHandler);
        }
    }
}