﻿namespace eShop.Services.Discount.DiscountAPI;
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
           .AddCommonErrorHandler(Configuration);

        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
        });


        services.AddScoped<IDiscountRepository, DiscountRepository>();
     

        services.AddMediatR(typeof(Startup).Assembly);

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
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Discount.API V1");
         
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
                using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "discount.proto"), FileMode.Open, FileAccess.Read);
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

            endpoints.MapGrpcService<DiscountService>();

        });
    }
}


public static class CustomExtensionMethods
{
    public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers((options) => {
           // options.Filters.Add(typeof(HttpGlobalExceptionFilter));
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
                Title = "eShop - Discount HTTP API",
                Version = "v1",
                Description = "The Discount Microservice HTTP API. This is a Data-Driven/CRUD microservice sample"
            });
        });

        return services;

    }


   

    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DiscountSetting>(configuration);
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
                    Detail =  "Please refer to the errors property for additional details."
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json", "application/problem+xml" }
                };
            };
        });


        return services;
    }


}
