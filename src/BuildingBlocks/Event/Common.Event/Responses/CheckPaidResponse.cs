namespace eShop.BuildingBlocks.Event.CommonEvent.Responses;

public class CheckPaidResponse
{
    [JsonConstructor]
    public CheckPaidResponse(int OrderId, Guid OrderNumber,bool IsPaid,string TransactionId, string Reason)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.IsPaid = IsPaid;
        this.TransactionId = TransactionId;
        this.Reason = Reason;
    }
    [JsonInclude]
    public int OrderId { get;private  set; }
    [JsonInclude]
    public Guid OrderNumber { get; private set; }
    [JsonInclude]
    public bool IsPaid { get; private set; }
    [JsonInclude]
    public string Reason { get; private set; }
    [JsonInclude]
    public string TransactionId { get; private set; }
}
