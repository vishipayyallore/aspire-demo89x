# Aspire Service Addition

**Purpose**: Add a new microservice to the Aspire application

**When to use**: When adding a new service project to the solution

## Prompt Template

```
Add a new [SERVICE_TYPE] service called [SERVICE_NAME] to the Aspire application. 

Requirements:
- Create the service project in src/HelloAspireApp.[SERVICE_NAME]/
- Use service defaults from HelloAspireApp.ServiceDefaults
- Add health check endpoint at /health
- Configure proper OpenTelemetry instrumentation
- Add reference to AppHost with custom resource naming
- Follow the existing project structure and conventions

Service details:
- [DESCRIBE_SERVICE_PURPOSE]
- [LIST_ANY_DEPENDENCIES]
- [SPECIFY_ENDPOINTS_OR_FEATURES]
```

## Example Usage

```
Add a new Web API service called OrderService to the Aspire application.

Requirements:
- Create the service project in src/HelloAspireApp.OrderService/
- Use service defaults from HelloAspireApp.ServiceDefaults
- Add health check endpoint at /health
- Configure proper OpenTelemetry instrumentation
- Add reference to AppHost with custom resource naming
- Follow the existing project structure and conventions

Service details:
- Handles order management operations
- Depends on Redis cache and ApiService
- Exposes REST endpoints for CRUD operations on orders
```
