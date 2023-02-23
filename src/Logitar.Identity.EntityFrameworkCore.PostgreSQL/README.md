# Logitar.Identity.EntityFrameworkCore.PostgreSQL

Provides an identity store implementation using the EntityFrameworkCore PostgreSQL provider.

## Migrations

This project is setup to use migrations. The following commands must be executed in the solution directory.

### Create a new migration

Do not forget to specify a migration name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --startup-project src/Logitar.Identity.Demo --project src/Logitar.Identity.EntityFrameworkCore.PostgreSQL --context IdentityContext`

### Remove the latest migration

`dotnet ef migrations remove --startup-project src/Logitar.Identity.Demo --project src/Logitar.Identity.EntityFrameworkCore.PostgreSQL --context IdentityContext`
