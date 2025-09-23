using Aspire.Hosting.Azure;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("env");

// Register the custom infrastructure resolver for fixed Azure resource names
builder.Services.Configure<AzureProvisioningOptions>(options =>
{
    options.ProvisioningBuildOptions.InfrastructureResolvers.Insert(0, new FixedNameInfrastructureResolver(builder.Configuration));
});

// What's the existing Redis Cache name and resource group?
var existingRedisName = builder.AddParameter("existingRedisName");
var existingRedisResourceGroup = builder.AddParameter("existingRedisResourceGroup");

var cache = builder.AddAzureRedis("cache")
    .AsExisting(existingRedisName, existingRedisResourceGroup);

var apiService = builder.AddProject<Projects.HelloAspireApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.HelloAspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
