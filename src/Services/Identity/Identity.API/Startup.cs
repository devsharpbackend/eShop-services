
namespace eShop.Services.IdentityAPI;

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
        services
            .AddCustomOptions(Configuration)
            .AddCustomMVC(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        var pathBase = Configuration["PATH_BASE"];
        if (!string.IsNullOrEmpty(pathBase))
        {       
            app.UsePathBase(pathBase);
        }

        app.UseStaticFiles();

        // Make work identity server redirections in Edge and lastest versions of browsers. WARN: Not valid in a production environment.
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
            await next();
        });

        app.UseForwardedHeaders();
        // Adds IdentityServer
        app.UseIdentityServer();

        // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
        // the cookies should be expired from https, but in eShop, the internal communication in aks and docker compose is http.
        // To avoid this problem, the policy of cookies should be in Lax mode.
        app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax });
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
           
        });
    }
}

public static class CustomExtensionMethods
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
       

        services.Configure<AppSettings>(configuration);

        return services;
    }

    public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddControllersWithViews();
        services.AddRazorPages();
        return services;
    }

  

 
}