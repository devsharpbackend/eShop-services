
namespace eShop.Services.CatalogAPI.Applicatioin.Behaviors;
public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly CatalogContext _dbContext;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

    public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, CatalogContext dbContext,
        ICatalogIntegrationEventService catalogIntegrationEventService
        )
    {
        _logger = logger?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (_dbContext.HasActiveTransaction)
            {
                return await next();
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();
           
            await strategy.ExecuteAsync(async () =>
            {

                Guid transactionId;
                using var transaction = await _dbContext.BeginTransactionAsync();
                using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                {
                    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _dbContext.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                }

                await _catalogIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);

            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }


}

public interface ITransactionBehaviour
{

}
