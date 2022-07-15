namespace eShop.BuildingBlocks.Event.ZeroMqEvent;

internal class ZeroMqPublisher : IZeroMqPublisher
{
    private readonly ZeroMqConfig _zeroMqConfig;
    public ZeroMqPublisher(ZeroMqConfig zeroMqConfig)
    {
        _zeroMqConfig = zeroMqConfig;
    }

    public void Publish(string subjectName, IntegrationEvent @event)
    {
        _zeroMqConfig.Publish(subjectName, @event);
    }

    public void Publish( IntegrationEvent @event)
    {
        _zeroMqConfig.Publish(@event);
    }
}

public interface IZeroMqPublisher
{
    public void Publish(string subjectName, IntegrationEvent @event);
    public void Publish(IntegrationEvent @event);

}