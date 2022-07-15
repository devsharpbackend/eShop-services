namespace eShop.Services.CatalogAPI.Applicatioin.Behaviors;


public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly CatalogErrorHandler _catalogErrorHandler;
    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger, CatalogErrorHandler catalogErrorHandler)
    {
        _logger = logger;
        _catalogErrorHandler = catalogErrorHandler;
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

            throw new CatalogApplicationException(ex, _catalogErrorHandler);

           
        }
    }
}