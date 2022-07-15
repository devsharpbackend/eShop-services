

namespace eShop.Services.Basket.BasketAPI;
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
            .AddGrpcServices(Configuration)
            .AddCommonErrorHandler(Configuration)
            .AddEventBusMassTransit(Configuration);


        services.AddHttpContextAccessor();
        services.AddScoped<IBasketIdentityService, BasketIdentityService>();

        services.AddScoped<BasketErrorHandler>();

        services.AddSingleton<ConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;

            var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);

            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddTransient(typeof(ExceptionBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddMediatR(typeof(Startup).Assembly);

        //services.AddZeroMq(cfg =>
        //{
        //    cfg.AddConsumer<CatalogPriceChangedEventHandler, CatalogPriceChangedIntegrationEvent>(Configuration["CatalogZerqUrl"]);
        //});
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
                     c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Basket.API V1");

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
                using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "basket.proto"), FileMode.Open, FileAccess.Read);
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

            endpoints.MapGrpcService<BasketService>();
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



        return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "eShop - Basket HTTP API",
                Version = "v1",
                Description = "The Basket Microservice HTTP API. This is a Data-Driven/CRUD microservice sample"
            });
        });

        return services;

    }
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BasketSettings>(configuration);
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


        return services;
    }

    public static IServiceCollection AddEventBusMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Publish<IntegrationEvent>(p =>
                {
                    p.Exclude = true;
                });
                cfg.Publish<IntegrationMessage>(p =>
                {
                    p.Exclude = true;
                });
                cfg.Publish<IntegrationCommand>(p =>
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


}
