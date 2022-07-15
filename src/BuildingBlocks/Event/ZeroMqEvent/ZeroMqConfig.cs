namespace eShop.BuildingBlocks.Event.ZeroMqEvent;

public interface IZeroMqConfig
{
    void AddConsumer<TMqConsumer, TMessage>(string host, string subscriptionClientName="") where TMqConsumer : IZeroMqConsumer where TMessage : IntegrationEvent;
    void ConfigPublisher(string host);
    void Dispose();

}

internal class ZeroMqConfig : IDisposable, IZeroMqConfig
{
    private readonly IServiceProvider _provider;

    private PublisherSocket? Publisher = null;

    public ZeroMqConfig(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void AddConsumer<TMqConsumer, TMessage>(string host, string subscriptionClientName="") where TMqConsumer : IZeroMqConsumer where TMessage : IntegrationEvent
    {
        if(string.IsNullOrEmpty(subscriptionClientName))
        {
            subscriptionClientName = typeof(TMessage).Name;
        }
        Task.Run(
           () =>
           {
               using var subSocket = new SubscriberSocket();

               subSocket.Options.ReceiveHighWatermark = 1000;
               subSocket.Connect(host);

               subSocket.Subscribe(subscriptionClientName);

               while (true)
               {
                   string messageTopicReceived = subSocket.ReceiveFrameString();
                   string messageReceived = subSocket.ReceiveFrameString();
                   var @event = JsonSerializer.Deserialize<TMessage>(messageReceived);

                   if (@event != null)
                   {
                       var ob = ActivatorUtilities.CreateInstance<TMqConsumer>(_provider) as IZeroMqConsumer<TMessage>;
                       if (ob != null)
                       {
                           ob?.Consume(@event);
                       }
                   }
               }
           }
         );
    }

    public void ConfigPublisher(string host)
    {
        if (host == null)
            throw new ArgumentNullException("host");

        Publisher = new PublisherSocket();

        Publisher.Options.SendHighWatermark = 1000;
        Publisher.Bind(host);
    }


    public void Publish(string subjectName, IntegrationEvent @event)
    {
        if (Publisher == null)
            throw new ArgumentNullException("Publisher Not Configure");

        string msg = JsonSerializer.Serialize(@event,@event.GetType());

        var retry = Policy.Handle<Exception>()
                       .WaitAndRetry(new TimeSpan[]
                       {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                       });

        retry.Execute(() =>
        {
            Publisher?.SendMoreFrame(subjectName).SendFrame(msg);
            Thread.Sleep(500);
        });
    }


    public void Publish(IntegrationEvent @event)
    {
        if (Publisher == null)
            throw new ArgumentNullException("Publisher Not Configure");

        string msg = JsonSerializer.Serialize(@event, @event.GetType());

        var retry = Policy.Handle<Exception>()
                       .WaitAndRetry(new TimeSpan[]
                       {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                       });

        retry.Execute(() =>
        {
         
            Publisher?.SendMoreFrame(@event.GetType().Name).SendFrame(msg);
            Thread.Sleep(500);
        });
    }



    public void Dispose()
    {
        Publisher?.Dispose();
    }
}