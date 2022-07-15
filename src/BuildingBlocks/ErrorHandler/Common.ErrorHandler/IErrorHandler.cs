namespace eShop.BuildingBlocks.Common.ErrorHandler;


public interface IErrorHandler
{
    JsonErrorResponse GetError(Exception ex);
}
