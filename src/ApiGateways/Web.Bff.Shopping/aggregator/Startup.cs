using CatalogGrpc;
using eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure;
using eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCustomMvc(Configuration)
            .AddApplicationServices()
            .AddGrpcServices();
        

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

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseSwagger().UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Purchase BFF V1");
            c.SwaggerEndpoint($"/b/swagger/v1/swagger.json", "Basket.API V1");
            c.SwaggerEndpoint($"/o/swagger/v1/swagger.json", "Ordering.API V1");
            c.SwaggerEndpoint($"/c/swagger/v1/swagger.json", "Catalog.API V1");
            c.SwaggerEndpoint($"/d/swagger/v1/swagger.json", "Discount.API V1");
        });
  
        app.UseRouting();
        app.UseCors("CorsPolicy");
     

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
           
        });

      
    }
}

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<UrlsConfig>(configuration.GetSection("urls"));

        services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddSwaggerGen(options =>
        {
           

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Shopping Aggregator for Web Clients",
                Version = "v1",
                Description = "Shopping Aggregator for Web Clients"
            });

            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows()
            //    {
            //        Implicit = new OpenApiOAuthFlow()
            //        {
            //            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
            //            TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),

            //            Scopes = new Dictionary<string, string>()
            //            {
            //                { "webshoppingagg", "Shopping Aggregator for Web Clients" }
            //            }
            //        }
            //    }
            //});

            //options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

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
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //register delegating handlers
       // services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //register http services

        return services;
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddTransient<GrpcExceptionInterceptor>();

        services.AddScoped<IBasketService, BasketService>();

        services.AddGrpcClient<Basket.BasketClient>((services, options) =>
        {
            var basketApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcBasket;
            options.Address = new Uri(basketApi);
        }).AddInterceptor<GrpcExceptionInterceptor>();

        services.AddScoped<ICatalogService, CatalogService>();

        services.AddGrpcClient<Catalog.CatalogClient>((services, options) =>
        {
            var catalogApi = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcCatalog;
            options.Address = new Uri(catalogApi);
        }).AddInterceptor<GrpcExceptionInterceptor>();

     

        return services;
    }

}
