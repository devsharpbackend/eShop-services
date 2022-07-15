using eShop.BuildingBlocks.Event.CommonEvent;
namespace eShop.Services.Catalog.CatalogAPI;
public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddCustomMVC(Configuration)
            .AddSwagger(Configuration)
            .AddCustomOptions(Configuration)
            .AddDomain(Configuration)
            .AddInfrastructure(Configuration)
            .AddCommonErrorHandler(Configuration)
            .AddGrpcServices(Configuration)
            .AddEventBusMassTransit(Configuration)
            .AddIIntegrationEventLogService(Configuration);

        if (Configuration["UseGrpc"] == "true")
        {
            services.AddScoped<IDiscountService, DiscountGrpcService>();
        }
        else
        {
            services.AddHttpClient<IDiscountService, DiscountService>()
               // Each time you get an HttpClient object from the IHttpClientFactory
               // , a new instance is returned. But each HttpClient uses an HttpMessageHandler
               // that's pooled and reused by the IHttpClientFactory to reduce resource consumption,
               // as long as the HttpMessageHandler's lifetime hasn't expired.
               .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                   .AddPolicyHandler(CustomExtensionMethods.GetRetryPolicy());
        }
        services.AddScoped<CatalogErrorHandler>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));



        services.AddMediatR(typeof(Startup).Assembly);



        services.AddHostedService<EventPublinsherWorker>();

        //services.AddZeroMq((cfg) =>
        // {
        //  cfg.AddConsumer<DiscountCreatedEventHandler1, DiscountCreatedIntegrationEvent>(Configuration["DiscountZerqUrl"]);
        //    cfg.ConfigPublisher("tcp://*:12345");

        // });



    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {

        var pathBase = Configuration["PATH_BASE"];

        if (!string.IsNullOrEmpty(pathBase))
        {
            loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            app.UsePathBase(pathBase);
        }

        app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.API V1");

             });


        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();

            endpoints.MapGet("/_proto/", async ctx =>
            {
                ctx.Response.ContentType = "text/plain";
                using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "catalog.proto"), FileMode.Open, FileAccess.Read);
                using var sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (line != "/* >>" || line != "<< */")
                    {
                        await ctx.Response.WriteAsync(line);
                    }
                }
            });

            endpoints.MapGrpcService<CatalogService>();

        });
    }
}


public static class CustomExtensionMethods
{
    public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers((options) =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShop - Catalog HTTP API",
                Version = "v1",
                Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample"
            });
        });

        return services;

    }




    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CatalogSetting>(configuration);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.ClientErrorMapping[404].Title = " Not Found Resouce Or Api ";
            options.ClientErrorMapping[403].Title = "Forbidden";
            options.InvalidModelStateResponseFactory = context =>
            {

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //services.AddFluentValidationAutoValidation(config =>
        //{
        //    config.DisableDataAnnotationsValidation = true;
        //});


        return services;
    }
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<LogServerInterceptor>();
            options.Interceptors.Add<GrpcServerExceptionInterceptor>();
        });



        services.AddTransient<ClientLoggingInterceptor>();
        services.AddTransient<GrpcClientExceptionInterceptor>();
        services.AddGrpcClient<CatalogDiscountGrpc.CatalogDiscountGrpcClient>((services, options) =>
        {
            options.Address = new Uri(configuration["DiscountGrpcUrl"]);
        }).AddInterceptor<ClientLoggingInterceptor>()
        .AddInterceptor<GrpcClientExceptionInterceptor>();


        return services;
    }


    public static IServiceCollection AddEventBusMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetExecutingAssembly());
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Publish<IntegrationEvent>(p =>
                {
                    p.Exclude = true;

                });
             

                cfg.UseDelayedMessageScheduler();
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }



    public static IServiceCollection AddIIntegrationEventLogService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
             sp => (DbConnection c) => new IntegrationEventLogService(c));

        services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        return services;
    }


}
