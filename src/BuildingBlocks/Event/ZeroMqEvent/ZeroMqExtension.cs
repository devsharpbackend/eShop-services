
namespace eShop.BuildingBlocks.Event.ZeroMqEvent;

public static class ZeroMqExtension
{
    public static IServiceCollection AddZeroMq(this IServiceCollection services, Action<IZeroMqConfig> action)
    {
        var config = new ZeroMqConfig(services.BuildServiceProvider());
      
        IZeroMqConfig Iconfig = config;
        action(Iconfig);

        services.AddScoped((serviceProvider) =>
        {
            return config;
        });

        services.AddScoped<IZeroMqPublisher>((serviceProvider) =>
        {
            return new ZeroMqPublisher(config);
        });

        return services;
    }
}
