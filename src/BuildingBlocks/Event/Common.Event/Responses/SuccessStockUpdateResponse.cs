namespace eShop.BuildingBlocks.Event.CommonEvent.Responses;

public class SuccessStockUpdateResponse
{
    [JsonConstructor]
    public SuccessStockUpdateResponse(int OrderId, Guid OrderNumber)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
       
      
    }
    [JsonInclude]
    public int OrderId { get;private  set; }
    [JsonInclude]
    public Guid OrderNumber { get; private set; }
  
  
}

