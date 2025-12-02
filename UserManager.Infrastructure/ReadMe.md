Migrations:

In order to update the local database schema according to the latest changes in the Entity Framework Core models, one can use the following steps:
- Open a terminal or command prompt.
- Navigate to the UserManager solution's root directory.
- Run the command:
  ```
  dotnet ef database update --project UserManager.Infrastructure --startup-project UserManager.Api
  ```

Alternatively, one can also use Package Manager Console, with proper commands for Entity Framework Core migrations.