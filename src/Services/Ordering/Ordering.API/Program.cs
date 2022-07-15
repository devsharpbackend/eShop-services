var configuration = GetConfiguration();

try
{
    var host = CreateHostBuilder(configuration, args);
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);


    host.MigrateDataBase<Program>((services) =>
    {
        var context = services.GetService<OrderingContext>();
        context.Database.Migrate();
    });


    host.MigrateDataBase<Program>((services) =>
    {
        var integrationEventLogContext = services.GetRequiredService<IntegrationEventLogContext>();

        integrationEventLogContext.Database.Migrate();

    });


    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    host.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}



IConfiguration GetConfiguration()
{
    var path = Directory.GetCurrentDirectory();

    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

IHost CreateHostBuilder(IConfiguration configuration, string[] args) =>

Host.CreateDefaultBuilder(args)
  .ConfigureLogging(logging =>
  {
      logging.AddFilter("Grpc", LogLevel.Debug);
  })
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>()
       .ConfigureKestrel(options =>
       {
           var ports = GetDefinedPorts(configuration);
           options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
           {
               listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
           });
           options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
           {
               listenOptions.Protocols = HttpProtocols.Http2;
           });
       })
      .UseContentRoot(Directory.GetCurrentDirectory())
      .UseWebRoot("Pics")
      .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
      .CaptureStartupErrors(false);
   })
   .UseSerilog(SeriLogger.Configure)
   .Build();





(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
{
    var grpcPort = config.GetValue("GRPC_PORT", 81);
    var port = config.GetValue("PORT", 80);
    return (port, grpcPort);
}




public partial class Program
{
    public static string? Namespace = typeof(Startup).Namespace;
    public static string? AppName = "Ordering.API";
}
