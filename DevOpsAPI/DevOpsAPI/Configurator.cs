using Amazon.Runtime;
using DevOpsAPI.Infra;
using DevOpsAPI.Statics;

namespace DevOpsAPI;

public static class Configurator
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        if (Config.Yandex.UseCloud)
            ConfigureCloud(services);
        else
            ConfigureLocal(services);
        
        return services;
    }

    private static void ConfigureCloud(IServiceCollection services)
    {
        services.AddScoped<BasicAWSCredentials>(_ => new BasicAWSCredentials(
            Config.Yandex.AccountAccessKey,
            Config.Yandex.AccountSecretKey));
        services.AddScoped<IStaticsService, StaticsService>();
        services.AddScoped<IQueueService, QueueService>();
    }

    private static void ConfigureLocal(IServiceCollection services)
    {
        services.AddScoped<IStaticsService, LocalStaticsService>();
        services.AddScoped<IQueueService>(_ => null);
    }
}