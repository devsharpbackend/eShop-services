namespace eShop.Services.Basket.BasketAPI.Applicatioin.Exceptions;

public class BasketApplicationException : Exception
{
    public BasketApplicationException(JsonErrorResponse jsonErrorResponse)
    {
        JsonErrorResponse = jsonErrorResponse;
    }
    public BasketApplicationException(Exception ex, BasketErrorHandler catalogErrorHandler)
    {
        JsonErrorResponse=catalogErrorHandler.GetError(ex);
    }
    public JsonErrorResponse JsonErrorResponse { get; private set; }
}
