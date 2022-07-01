
namespace eShop.BuildingBlocks.Logging.CommonLogging;
public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
       (context, logConfiguration) =>
       {
           var seqServerUrl = context.Configuration["SeqServerUrl"];
           var logstashUrl = context.Configuration["LogstashgUrl"];

           logConfiguration
              .MinimumLevel.Verbose()
              .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
              .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .WriteTo.File(new RenderedCompactJsonFormatter(), "log.ndjson", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
              //.WriteTo.File("log12.txt",
              // rollingInterval: RollingInterval.Infinite,
              // rollOnFileSizeLimit: true, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
              .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
              .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
              .ReadFrom.Configuration(context.Configuration);

       };
}
