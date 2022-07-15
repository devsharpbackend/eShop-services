namespace eShop.BuildingBlocks.Event.ZeroMqEvent;

public interface IZeroMqConsumer<in TMessage> : IZeroMqConsumer where TMessage : IntegrationEvent
{
    Task Consume(TMessage message);
}

public interface IZeroMqConsumer
{
}

