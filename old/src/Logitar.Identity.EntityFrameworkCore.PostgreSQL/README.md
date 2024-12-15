# Logitar.Identity.EntityFrameworkCore.PostgreSQL

Provides an implementation of a relational event store to be used with Identity management platform, Entity Framework Core and PostgreSQL.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to provide a migration name!

```sh
dotnet ef migrations add <YOUR_MIGRATION_NAME> --context IdentityContext --project src/Logitar.Identity.EntityFrameworkCore.PostgreSQL --startup-project src/Logitar.Identity.Demo
```

### Remove a migration

To remove the latest unapplied migration, execute the following command.

```sh
dotnet ef migrations remove --context IdentityContext --project src/Logitar.Identity.EntityFrameworkCore.PostgreSQL --startup-project src/Logitar.Identity.Demo
```

### Generate a script

To generate a script, execute the following command. Do not forget to provide a source migration name!

```sh
dotnet ef migrations script <SOURCE_MIGRATION> --context IdentityContext --project src/Logitar.Identity.EntityFrameworkCore.PostgreSQL --startup-project src/Logitar.Identity.Demo
```
