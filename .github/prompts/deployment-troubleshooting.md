# Deployment Troubleshooting

**Purpose**: Debug and resolve deployment issues with Azure Container Apps

**When to use**: When azd up fails or resources aren't created correctly

## Prompt Template

```text
Help troubleshoot the deployment issue with the Aspire application.

Current situation:
- [DESCRIBE_WHAT_HAPPENED]
- [INCLUDE_ERROR_MESSAGES]
- [LIST_STEPS_ATTEMPTED]

Please:
1. Analyze the error and identify potential causes
2. Check infrastructure configuration and naming
3. Verify Azure resource dependencies
4. Provide step-by-step resolution steps
5. Suggest preventive measures for future deployments
```

## Common Issues and Solutions

### Resource Naming Conflicts

- Check Azure portal for existing resources with same names
- Verify AZURE_ENV_SUFFIX is set correctly
- Review FixedNameInfrastructureResolver implementation

### Permission Issues

- Ensure proper Azure subscription access
- Verify Azure CLI authentication: `azd auth login`
- Check resource group permissions

### Package Version Conflicts

- Review Directory.Packages.props for version mismatches
- Ensure all Aspire packages use compatible versions
- Check for transitive dependency conflicts

## Example Usage

```text
Help troubleshoot the deployment issue with the Aspire application.

Current situation:
- azd up fails with "resource already exists" error
- Container registry creation is failing
- Error mentions "svacrd" already exists in resource group

Please:
1. Analyze the error and identify potential causes
2. Check infrastructure configuration and naming
3. Verify Azure resource dependencies
4. Provide step-by-step resolution steps
5. Suggest preventive measures for future deployments
```
