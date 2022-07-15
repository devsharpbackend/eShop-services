
namespace eShop.BuildingBlocks.Common.ErrorHandler;

public class JsonErrorResponse
{
    public string[]? Messages { get; set; }

    public object? DeveloperMessage { get; set; }

    public int StatusCode { get; set; } = 500;
}
