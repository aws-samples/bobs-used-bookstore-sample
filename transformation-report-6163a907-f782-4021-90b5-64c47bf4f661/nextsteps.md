# Next Steps

## Issues resolved
- Transformed Bookstore.Domain.csproj to net8.0
- Transformed Bookstore.Data.csproj to net8.0
- Transformed Bookstore.Web.csproj to net8.0
- Transformed Bookstore.Cdk.csproj to net8.0
- Transformed Bookstore.Domain.Tests.csproj to net8.0

## Overview

The solution build produced no errors across all five projects after transformation:

- `Bookstore.Data`
- `Bookstore.Domain.Tests`
- `Bookstore.Cdk`
- `Bookstore.Web`
- `Bookstore.Domain`

This indicates the migration to cross-platform .NET was successful. The following steps outline how to validate, test, and deploy the solution.

---

## 1. Restore and Build the Solution

Run the following commands from the solution root to confirm a clean restore and build:

```bash
dotnet restore
dotnet build --configuration Release
```

Verify that no warnings or errors appear in the output. Address any warnings that could indicate compatibility issues, such as deprecated API usage or nullable reference warnings.

---

## 2. Run the Unit Tests

Execute the test project to confirm all existing tests pass on the new runtime:

```bash
dotnet test app/Bookstore.Domain.Tests/Bookstore.Domain.Tests.csproj --configuration Release --verbosity normal
```

- Review the test output for any failures or skipped tests.
- If tests that previously passed are now failing, inspect whether the failures are related to platform-specific behavior, changed APIs, or dependency version mismatches.

---

## 3. Verify Runtime Behavior of the Web Project

Run the web application locally to confirm it starts and behaves correctly:

```bash
dotnet run --project app/Bookstore.Web/Bookstore.Web.csproj --configuration Release
```

- Navigate through the key pages and workflows in a browser.
- Check the application logs for any runtime exceptions or unhandled errors.
- Confirm that database connectivity works as expected by exercising features that depend on `Bookstore.Data`.

---

## 4. Validate the Data Layer

- Confirm that any Entity Framework Core migrations are up to date by running:

```bash
dotnet ef migrations list --project app/Bookstore.Data/Bookstore.Data.csproj --startup-project app/Bookstore.Web/Bookstore.Web.csproj
```

- If migrations are missing or out of sync, generate a new migration and apply it to a local database:

```bash
dotnet ef migrations add PostMigration --project app/Bookstore.Data/Bookstore.Data.csproj --startup-project app/Bookstore.Web/Bookstore.Web.csproj
dotnet ef database update --project app/Bookstore.Data/Bookstore.Data.csproj --startup-project app/Bookstore.Web/Bookstore.Web.csproj
```

---

## 5. Review the CDK Project

The `Bookstore.Cdk` project likely defines infrastructure. Confirm it synthesizes correctly:

```bash
dotnet build app/Bookstore.Cdk/Bookstore.Cdk.csproj --configuration Release
```

- If this project uses the AWS CDK, run `cdk synth` from the CDK project directory to validate the CloudFormation output:

```bash
cd app/Bookstore.Cdk
cdk synth
```

- Review the synthesized output for any resource definitions that may need to be updated to reflect the new .NET runtime version (for example, Lambda function runtime identifiers should reference `dotnet8` or the appropriate current target).

---

## 6. Check Target Framework and Dependency Versions

Open each `.csproj` file and confirm:

- The `<TargetFramework>` is set to the intended version (e.g., `net8.0`).
- NuGet package versions are consistent across projects and do not reference packages that are no longer maintained or that have known incompatibilities with the target framework.

You can audit outdated packages with:

```bash
dotnet list package --outdated
```

---

## 7. Publish the Application

Once validation is complete, publish the web application:

```bash
dotnet publish app/Bookstore.Web/Bookstore.Web.csproj --configuration Release --output ./publish
```

- Review the contents of the `./publish` directory to confirm all expected files are present.
- Test the published output by running it directly before deploying to the target environment.