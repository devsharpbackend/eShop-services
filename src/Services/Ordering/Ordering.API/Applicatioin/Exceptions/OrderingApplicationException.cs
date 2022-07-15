
namespace eShop.Services.Ordering.OrderingAPI.Application.Exceptions;

public class OrderingApplicationException : Exception
{
    public OrderingApplicationException(JsonErrorResponse jsonErrorRespons)
    {
        JsonErrorResponse = jsonErrorRespons;
    }
    public OrderingApplicationException(Exception ex, OrderingErrorHandler catalogErrorHandler)
    {
        JsonErrorResponse=catalogErrorHandler.GetError(ex);
    }
    public JsonErrorResponse JsonErrorResponse { get; private set; }
}