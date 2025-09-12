# GitHub Copilot Instructions for HelloAspireApp

## Project Overview

This is an ASP.NET Core Aspire application that demonstrates a microservices architecture with Azure Container Apps deployment and custom resource naming.

### Architecture Components

- **HelloAspireApp.AppHost**: Orchestration host with Azure Container Apps environment setup
- **HelloAspireApp.ApiService**: Web API service with health checks and OpenTelemetry
- **HelloAspireApp.Web**: Blazor Server application with Redis output caching
- **HelloAspireApp.ServiceDefaults**: Shared configuration for service defaults (OpenTelemetry, health checks, service discovery)

## Key Features

### 🏗️ **Azure Container Apps Infrastructure**

- Uses `Aspire.Hosting.Azure.AppContainers` package
- Custom `FixedNameInfrastructureResolver` for predictable resource naming
- Environment-based naming with configurable suffix
- Automatic infrastructure provisioning via Azure Developer CLI (azd)

### 🎯 **Custom Resource Naming Pattern**

```
Prefix: "sv"
Environment Suffix: Configurable (D=Development, P=Production, etc.)

Examples:
- Redis Cache: sv-cache-D
- Container App: sv-apiservice-D, sv-webfrontend-D
- Container Registry: svacrd
- Log Analytics: sv-law-D
- Container App Environment: sv-cae-D
```

### 📦 **Package Management**

- Uses Central Package Management (CPM) via Directory.Packages.props
- All package versions centrally managed
- No Version attributes in individual project files

## Development Guidelines

### When Adding New Services

1. Add project reference to AppHost
2. Use `builder.AddProject<Projects.NewService>("servicename")`
3. Ensure service implements service defaults via `builder.AddServiceDefaults()`
4. Add health check endpoints: `builder.WithHttpHealthCheck("/health")`

### When Adding Azure Resources

1. Update `FixedNameInfrastructureResolver.cs` to handle new resource types
2. Follow the naming convention: `{prefix}-{identifier}-{environmentSuffix}`
3. Add corresponding package references to Directory.Packages.props

### Environment Configuration

- Set `AZURE_ENV_SUFFIX` environment variable or configuration value
- Common values: D (Development), S (Staging), P (Production)
- Default fallback is "D" if not specified

### OpenTelemetry & Observability

- All services use shared OpenTelemetry configuration from ServiceDefaults
- Logging, metrics, and tracing are automatically configured
- Health check endpoints exclude telemetry to reduce noise

## Deployment

### Prerequisites

```bash
# Install Azure Developer CLI
winget install Microsoft.AzureDeveloperCLI

# Login to Azure
azd auth login
```

### Deploy to Azure

```bash
# Initialize (first time only)
azd init

# Deploy infrastructure and applications
azd up
```

### Local Development

```bash
# Build solution
dotnet build

# Run the AppHost (starts all services)
dotnet run --project src/HelloAspireApp.AppHost
```

## Common Patterns

### Adding a New Microservice

```csharp
// In AppHost.cs
var newService = builder.AddProject<Projects.NewService>("newservice")
    .WithHttpHealthCheck("/health");

// Reference from other services
builder.AddProject<Projects.ExistingService>("existing")
    .WithReference(newService)
    .WaitFor(newService);
```

### Adding External Dependencies

```csharp
// Redis example
var cache = builder.AddRedis("cache");

// SQL Server example
var database = builder.AddSqlServer("sql")
    .AddDatabase("mydb");
```

### Service Configuration

```csharp
// In service Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add service defaults (required for all services)
builder.AddServiceDefaults();

// Add other services...
builder.Services.AddControllers();

var app = builder.Build();

// Map default endpoints (health checks, etc.)
app.MapDefaultEndpoints();
```

## Troubleshooting

### Build Issues

- Check Directory.Packages.props for version conflicts
- Ensure no Version attributes in PackageReference elements
- Verify all projects target the same .NET version

### Deployment Issues

- Verify Azure CLI authentication: `azd auth login`
- Check resource naming conflicts in Azure portal
- Review azd logs for infrastructure provisioning errors

### Service Discovery Issues

- Ensure all services call `builder.AddServiceDefaults()`
- Verify service names match between AppHost and service references
- Check that services are properly registered in AppHost.cs

## File Structure

```
aspire-demo89x/
├── src/
│   ├── HelloAspireApp.AppHost/          # Orchestration and infrastructure
│   ├── HelloAspireApp.ApiService/       # Web API service
│   ├── HelloAspireApp.Web/              # Blazor frontend
│   └── HelloAspireApp.ServiceDefaults/  # Shared service configuration
├── Directory.Packages.props             # Central package management
├── Directory.Build.props                # Build configuration
├── azure.yaml                          # Azure Developer CLI configuration
└── README.md                           # Project documentation
```

## Best Practices

1. **Always use service defaults** in every service for consistent configuration
2. **Follow naming conventions** when adding new Azure resources
3. **Use health checks** for all services to enable proper orchestration
4. **Configure environment-specific settings** via configuration, not code
5. **Use Central Package Management** for consistent dependency versions
6. **Test locally** before deploying to Azure
7. **Monitor resource naming** to avoid conflicts in shared Azure subscriptions
