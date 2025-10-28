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
- (Optional) Visual Studio 2022/2023 or VS Code

Quick Start (local)
1. Clone the repository
   git clone https://github.com/ShelarAtharva404/Online-Quiz-managment-app.git
   cd Online-Quiz-managment-app/QuizPortal

2. Restore dependencies
   dotnet restore

3. Configure the database connection (see next section)

4. Apply EF Core migrations to create the database
   dotnet ef database update

5. Run the app
   dotnet run

6. Open the browser
   http://localhost:5000 or https://localhost:5001

Configuration
- appsettings.json
  - Update the DefaultConnection string to point to your SQL Server instance.
  Example:
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QuizPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
    },
    "Logging": { ... }
  }
- If using a remote SQL server, replace Server and credentials appropriately.

Database & Migrations (EF Core)
- Install the EF global tool (if you haven't already):
  dotnet tool install --global dotnet-ef
  dotnet ef --version

- Add required EF packages (already present in the project, shown here for reference):
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10

- Create a migration (only if you modify models):
  dotnet ef migrations add <MigrationName>

- Apply migrations (creates or updates database schema):
  dotnet ef database update

- Remove last migration (if needed before applying):
  dotnet ef migrations remove

- View SQL script for migrations:
  dotnet ef migrations script

Default Accounts & Roles
- The project seeds example roles and users (if seeding is enabled). Example credentials (update these to match your seeded data or seed your own):
  - Professor / Admin
    Email: admin@example.com
    Password: Admin@123
  - Student
    Email: student@example.com
    Password: Student@123



Common Commands & Troubleshooting
- dotnet run — runs the app
- dotnet ef database update — applies EF migrations
- If you get EF package version conflicts, align all EF packages to the same version (8.0.10 in this project):
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
  dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10

- SQL Server connection issues:
  - Ensure SQL Server or LocalDB is running.
  - Confirm the connection string server name and credentials.
  - Add TrustServerCertificate=True if using self-signed certs.

- Migration errors (missing columns or mismatched schema):
  - If you changed models: add a migration and run database update.
  - If migrations are out-of-sync in development, you may need to recreate the database (only in non-production):
    1. Remove database
    2. dotnet ef database update

Security & Notes
- Change seeded passwords before using this in any public/demo environment.
- For production, use secure secrets storage (e.g., Azure Key Vault, environment vars) instead of plaintext appsettings.json.
- Consider HTTPS, strong password rules, account lockout, and anti-forgery tokens for production readiness.

Contributing
- This project is intended for learning and college projects. Contributions and improvements are welcome:
  - Open issues for bugs/feature requests
  - Fork and submit pull requests with clear summaries of changes

Author & Contact
- Developed by: Atharva Shelar
- Contact: your-23amtics404@gmail.com
- For academic use / college project


