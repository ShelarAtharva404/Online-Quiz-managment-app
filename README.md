# Quiz Portal — ASP.NET Core MVC

A simple, extensible online quiz management application built with ASP.NET Core MVC and Entity Framework Core. This project is intended for academic/demo use and demonstrates user roles (Professor/Admin and Student), CRUD for quizzes and questions, quiz-taking workflows, automatic scoring, and basic result management.

---

Table of contents
- [Demo & Purpose](#demo--purpose)
- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Database & Migrations (EF Core)](#database--migrations-ef-core)
- [Default Accounts & Roles](#default-accounts--roles)
- [Project Structure](#project-structure)
- [Common Commands & Troubleshooting](#common-commands--troubleshooting)
- [Contributing](#contributing)
- [License & Author](#license--author)

---

Demo & Purpose
- Purpose: Provide a lightweight example of a quiz system where instructors can create quizzes and students can take them and see results.
- Use cases: college project, learning how to use ASP.NET Core MVC + EF Core, prototyping quiz features.

Key Features
- Professor/Admin dashboard to create, edit, and delete:
  - Quizzes
  - Questions (multiple choice)
  - Quiz assignment/publishing
- Student dashboard to:
  - Register / Login
  - Browse available quizzes
  - Take timed quizzes
  - View immediate score and history
- Automatic scoring and result storage
- Authentication and role-based authorization
- Uses Entity Framework Core (Code-First) with SQL Server
- Responsive UI with Razor Views and Bootstrap

Tech Stack
- .NET SDK: 8.0 (recommended)
- ASP.NET Core MVC
- Entity Framework Core 8.0.10 (EF Core SqlServer, Tools, Design)
- Microsoft SQL Server (or LocalDB)
- AutoMapper 12.x
- Razor views + Bootstrap 5

Prerequisites
- .NET 8 SDK — check with:
  dotnet --version
- SQL Server or LocalDB running locally

## Installation — step by step

1. Clone the repository
```
git clone https://github.com/ShelarAtharva404/Online-Quiz-managment-app.git
cd Online-Quiz-managment-app/QuizPortal
```

2. Verify .NET SDK
```
dotnet --version
# should be 8.x.x
```

3. Restore NuGet packages
```
dotnet restore
```

4. Install EF Core CLI tool (if you plan to run migrations locally)
- Option A — Global install (one time)
```
dotnet tool install --global dotnet-ef
```
- Option B — Local tool manifest (recommended for reproducible builds)
```
dotnet new tool-manifest # if you don't already have one
dotnet tool install dotnet-ef
```
Confirm installation:
```
dotnet ef --version
```

5. Configure your connection string
- Edit `appsettings.json` or add an `appsettings.Development.json` (recommended for local dev).
Example (LocalDB):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QuizPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```
Example (SQL Server with credentials):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=QuizPortalDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}
```

6. Apply EF Core migrations (create database & schema)
- From the project root (where the .csproj is), run:
```
dotnet ef database update
```

7. Run the application
```
dotnet run
```
Open:
- http://localhost:5000
- https://localhost:5001

---

## Configuration tips

- Environment:
  - Use `ASPNETCORE_ENVIRONMENT=Development` locally to load `appsettings.Development.json`.
  - Example on Windows (PowerShell):
    ```
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet run
    ```
  - On macOS / Linux:
    ```
    ASPNETCORE_ENVIRONMENT=Development dotnet run
    ```

- Overriding connection string without editing appsettings:
  - You can set environment variable `ConnectionStrings__DefaultConnection` to override the connection string for local runs.

---

## Entity Framework Core — commands explained (detailed)

Here is a concise catalog of the EF Core commands you'll use during development, with what they do and examples.

1) Add EF tool (done once)
```
dotnet tool install --global dotnet-ef
```
What it does: Installs `dotnet-ef` so you can run migration and database commands.

2) Add EF Core packages (one-time or when upgrading)
These are usually already included in the project, but for reference:
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
```
Why: SqlServer provider and design-time tooling for generating migrations.

3) Create a new migration
```
dotnet ef migrations add InitialCreate
```
What it does: Scaffolds C# migration files (in Migrations/) based on your current DbContext and models. Naming convention: use descriptive names like `AddScoreMarks` or `RenameQuestionText`.

Helpful options:
- `--output-dir Migrations/SomeFolder`  — place migration files in a specific folder
- `--context ApplicationDbContext` — if you have multiple DbContexts

4) Apply migrations to the database (create/update DB)
```
dotnet ef database update
```
What it does: Runs pending migrations against the configured database, bringing schema up to date.

Apply a specific migration:
```
dotnet ef database update InitialCreate
```
This will update the DB to the state of the given migration (useful for rolling back to a previous migration).

5) Remove the last migration (before applying to DB)
```
dotnet ef migrations remove
```
What it does: Removes the last scaffolded migration file. Use this only if you haven't applied it to the database or if you will revert the DB manually.

6) Generate SQL script for migrations
- Full script from initial -> latest:
```
dotnet ef migrations script -o migrations.sql
```
- Script between two migrations:
```
dotnet ef migrations script FromMigration ToMigration --output script.sql
```
- Idempotent script (safe to run on DB with unknown current state):
```
dotnet ef migrations script --idempotent -o idempotent.sql
```
Why: Use scripts for DBAs or production deployments where you apply SQL rather than letting the app run automatic migrations.

7) Useful flags when running commands in multi-project solutions
- `--project <PROJECT_PATH>` — the project that contains the migrations (e.g., path to the class library)
- `--startup-project <STARTUP_PROJECT_PATH>` — the project used to run the app (provides configuration)
Example:
```
dotnet ef migrations add AddMarksToScore --project ./QuizPortal --startup-project ./QuizPortal
```

8) Reverting database to a previous migration
```
dotnet ef database update PreviousMigrationName
```
If you want to revert to an empty database:
```
dotnet ef database update 0
```

9) Verbose output & diagnosing issues
```
dotnet ef migrations add Name --verbose
```
This prints build and toolchain details to help debug problems.

---

## Common EF scenarios & explanations

- Migrations included in repo:
  If the repository includes a `Migrations/` folder, you can usually skip `migrations add` and directly run `dotnet ef database update` to create the database.

- Model change workflow:
  1. Modify model class (e.g., add property `Marks` to `Score`).
  2. Add migration:
     ```
     dotnet ef migrations add AddMarksToScore
     ```
  3. Apply migration:
     ```
     dotnet ef database update
     ```

- Accidentally added wrong migration:
  - If not applied to DB: `dotnet ef migrations remove`
  - If already applied: create a new migration that reverses or adjust manually, or revert DB to prior migration then remove the file.

- Handling migration conflicts across branches:
  - Avoid multiple branches each adding migrations with the same base. If you merge, you may need to:
    - Rebase migrations into a single linear sequence, or
    - Add a new migration that reconciles differences after merging.

---

## Seeding default roles and users
- The project may automatically seed roles (Admin/Professor/Student) and example users during startup (check Data/ folder).
- If seeding is present, the simplest way to ensure seeded accounts exist is to run the app after migrations:
```
dotnet run
``

Always change seeded credentials before publishing or demoing publicly.

---


Update your connection string to point to the Docker host (from the app container use `sqlserver` as host):
```
"DefaultConnection": "Server=sqlserver,1433;Database=QuizPortalDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
```

If you run the app on the host machine and the DB is in Docker, use `localhost` or `host.docker.internal` depending on your OS.

---

## Troubleshooting & common fixes

- EF package downgrade / version conflicts
  - Ensure all EF packages use the same version (8.0.10 in this project).
  ```
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
  ```

- "No migrations found" or DbContext not found
  - Ensure you run `dotnet ef` from the project containing the DbContext or use `--project` / `--startup-project` flags.

- SQL Server connection errors
  - Make sure SQL Server service is running or Docker container is up.
  - Confirm server name, port, credentials.
  - If using self-signed TLS, set `TrustServerCertificate=True` in connection string.

- Migration already applied but model mismatch
  - Add a new migration that updates schema to match model.
  - Alternatively, revert DB to previous migration state and re-apply migrations in order if appropriate.

- "Permission denied" on Linux for LocalDB
  - LocalDB is Windows-only. Use SQL Server on Linux or Docker.

---

## Quick reference — common command list

- Clone & restore:
```
git clone <repo-url>
cd QuizPortal
dotnet restore
```
- Install EF tool (global):
```
dotnet tool install --global dotnet-ef
```
- Add migration:
```
dotnet ef migrations add <MigrationName>
```
- Apply migration:
```
dotnet ef database update
```
- Remove last migration (before apply):
```
dotnet ef migrations remove
```
- Generate SQL script:
```
dotnet ef migrations script -o migrations.sql
dotnet ef migrations script --idempotent -o idempotent.sql
```
- Apply migrations with explicit projects:
```
dotnet ef database update --project ./QuizPortal --startup-project ./QuizPortal
```

---

## Project structure
```
QuizPortal/
├── Controllers/      — MVC controllers and endpoints
├── Models/           — Entity models and DTOs
├── Views/            — Razor Views (UI)
├── Data/             — ApplicationDbContext and seeders
├── Migrations/       — EF Core generated migrations
├── wwwroot/          — Static assets (CSS, JS, images)
└── appsettings.json  — Configuration including DB connection string
```

---

## Security & production notes
- Do not keep production passwords in appsettings.json. Use environment variables or a secrets manager (Azure Key Vault, AWS Secrets Manager).
- Use strong password policies, account lockout, HTTPS, and anti-forgery tokens in production.
- Consider using CI/CD with scripted migration application or DBA-reviewed SQL scripts.

---

## Author
- Developed by: Atharva Shelar
- Contact: your-23amtics404@gmail.com
- For academic / college project use

---



