# Azure Resource Addition

**Purpose**: Add a new Azure resource to the Aspire infrastructure

**When to use**: When adding new Azure services like databases, storage, messaging, etc.

## Prompt Template

```text
Add [AZURE_RESOURCE_TYPE] support to the Aspire application infrastructure.

Requirements:
- Update FixedNameInfrastructureResolver.cs to handle [RESOURCE_TYPE] naming
- Follow naming convention: sv-[identifier]-[environmentSuffix]
- Add required Azure Provisioning packages to Directory.Packages.props
- Update AppHost.cs to configure the new resource
- Ensure proper service references and wait dependencies

Resource details:
- [DESCRIBE_RESOURCE_PURPOSE]
- [LIST_CONFIGURATION_REQUIREMENTS]
- [SPECIFY_DEPENDENT_SERVICES]
```

## Example Usage

```text
Add Azure SQL Database support to the Aspire application infrastructure.

Requirements:
- Update FixedNameInfrastructureResolver.cs to handle SqlServer naming
- Follow naming convention: sv-[identifier]-[environmentSuffix]
- Add required Azure Provisioning packages to Directory.Packages.props
- Update AppHost.cs to configure the new resource
- Ensure proper service references and wait dependencies

Resource details:
- Provides relational database storage for the application
- Requires connection string configuration for services
- ApiService and OrderService should depend on this database
```
