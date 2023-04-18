namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Exceptions;

public class CatalogApplicationException : Exception
{
    public CatalogApplicationException(Exception ex, CatalogErrorHandler catalogErrorHandler)
    {
        JsonErrorResponse=catalogErrorHandler.GetError(ex);
    }
    public JsonErrorResponse JsonErrorResponse { get; private set; }
}