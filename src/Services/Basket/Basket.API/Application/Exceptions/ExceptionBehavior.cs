
namespace eShop.Services.CatalogAPI.Applicatioin.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly BasketErrorHandler _basketErrorHandler;
    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger, BasketErrorHandler basketErrorHandler)
    {
        _logger = logger;
        _basketErrorHandler = basketErrorHandler;
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
            _logger.LogInformation("----- Error in  Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), ex.ToString());

            if(ex.GetType()==typeof(BasketApplicationException))
            {
                throw ex;
            }

            throw new BasketApplicationException(ex, _basketErrorHandler);

           
        }
    }
}