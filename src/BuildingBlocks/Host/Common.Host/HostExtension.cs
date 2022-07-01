using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;

namespace eShop.BuildingBlocks.Host.CommonHost;
public static class HostExtension
{
    public static bool IsInKubernetes(this IHost host)
    {
        var cfg = host.Services.GetService<IConfiguration>();
        var orchestratorType = cfg.GetValue<string>("OrchestratorType");
        return orchestratorType?.ToUpper() == "K8S";
    }

    public static IHost MigrateDataBase<TContext>(this IHost host, Action<IServiceProvider> MigrateAndSeed)
    {
        bool underK8s = host.IsInKubernetes();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                if (underK8s)
                {
                    MigrateAndSeed(services);
                }
                else
                {
                    var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);
                    var retry = Policy.Handle<Exception>().WaitAndRetry(delay);
                    retry.Execute(() =>
                    {
                        MigrateAndSeed(services);
                    });
                }

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                if (underK8s)
                {
                    throw;          // Rethrow under k8s because we rely on k8s to re-run the pod
                }
            }
        }

        return host;
    }

}

