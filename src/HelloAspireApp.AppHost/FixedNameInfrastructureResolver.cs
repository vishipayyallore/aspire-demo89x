using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.ContainerRegistry;
using Azure.Provisioning.OperationalInsights;
using Azure.Provisioning.Primitives;
using Azure.Provisioning.Resources;
using Microsoft.Extensions.Configuration;

internal class FixedNameInfrastructureResolver(IConfiguration configuration) : InfrastructureResolver
{
    private readonly IConfiguration _configuration = configuration;
    private const string UniqueNamePrefix = "sv";

    public override void ResolveProperties(ProvisionableConstruct construct, ProvisioningBuildOptions options)
    {
        // Get environment suffix from environment variable first, then config, default to "D" (Development)
        string environmentSuffix = Environment.GetEnvironmentVariable("AZURE_ENV_SUFFIX")
                                  ?? _configuration["AZURE_ENV_SUFFIX"]
                                  ?? "D";

        // Debug: Log the environment suffix being used
        Console.WriteLine($"[FixedNameInfrastructureResolver] Using environment suffix: {environmentSuffix} for {construct.GetType().Name}");

        switch (construct)
        {
            case Azure.Provisioning.Redis.RedisResource redisCache:
                redisCache.Name = $"{UniqueNamePrefix}-{redisCache.BicepIdentifier.ToLowerInvariant()}-{environmentSuffix}";
                break;

            case ContainerApp containerApp:
                containerApp.Name = $"{UniqueNamePrefix}-{containerApp.BicepIdentifier.ToLowerInvariant()}-{environmentSuffix}";
                break;

            case ContainerRegistryService containerRegistry:
                containerRegistry.Name = $"{UniqueNamePrefix}acr{environmentSuffix.ToLower()}";
                break;

            case OperationalInsightsWorkspace logAnalyticsWorkspace:
                logAnalyticsWorkspace.Name = $"{UniqueNamePrefix}-law-{environmentSuffix}";
                break;

            case ContainerAppManagedEnvironment containerAppEnvironment:
                containerAppEnvironment.Name = $"{UniqueNamePrefix}-cae-{environmentSuffix}";
                break;

            default:
                // For any other resource types, apply a generic naming pattern
                if (construct is NamedProvisionableConstruct namedConstruct && !string.IsNullOrEmpty(namedConstruct.BicepIdentifier))
                {
                    // Use reflection to set the Name property if it exists
                    var nameProperty = construct.GetType().GetProperty("Name");
                    if (nameProperty != null && nameProperty.CanWrite)
                    {
                        var genericName = $"{UniqueNamePrefix}-{namedConstruct.BicepIdentifier.ToLowerInvariant()}-{environmentSuffix}";
                        nameProperty.SetValue(construct, genericName);
                        Console.WriteLine($"[FixedNameInfrastructureResolver] Applied generic naming to {construct.GetType().Name}: {genericName}");
                    }
                }
                break;
        }
    }

}