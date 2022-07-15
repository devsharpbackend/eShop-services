namespace eShop.Services.Ordering.Infrastructure.Idempotency;

public class ClientRequest
{
    // Request Id
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
