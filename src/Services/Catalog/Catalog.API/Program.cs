var configuration = GetConfiguration();

var host = CreateHostBuilder(configuration,args);

host.Run();



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
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>()
       //.ConfigureKestrel(options =>
       //{
       //    options.Listen(IPAddress.Any, 80, listenOptions =>
       //    {
       //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
       //    });
       //})
      .UseContentRoot(Directory.GetCurrentDirectory())
      .UseWebRoot("Pics")
      .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
      .CaptureStartupErrors(false);
   })
   .Build();
