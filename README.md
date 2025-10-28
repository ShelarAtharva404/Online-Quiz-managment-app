🧩 Quiz Portal – ASP.NET Core MVC Project
📘 Overview
Quiz Portal is a web-based application built using ASP.NET Core MVC and Entity Framework Core that allows students to take quizzes online, while professors can create and evaluate quizzes.
It demonstrates CRUD operations, authentication, AJAX DataTables, and SQL Server database integration.
🚀 Features
•	🧑‍🏫 Professor Dashboard – Create and manage quizzes
•	🎓 Student Dashboard – Take quizzes and view scores
•	📊 Results Management – Auto-calculate and store scores
•	💾 Entity Framework Core (Code-First)
•	⚙️ ASP.NET Core MVC architecture
🧰 Technologies Used
Component---Version	Description
.NET SDK----8.0	ASP.NET Core MVC Framework
Entity Framework Core---8.0.10	ORM for SQL Server
Microsoft SQL Server----Database backend
AutoMapper	12.0.1---Object mapping
Razor Pages---Frontend templating
Bootstrap---5.x	UI styling
⚙️ Installation & Setup
1. Clone the Repository
git clone https://github.com/<your-username>/quiz-portal.git
cd quiz-portal/QuizPortal
2. Check .NET SDK
Make sure .NET 8 SDK is installed:
dotnet --version
If not, download from: https://dotnet.microsoft.com/download
3. Restore Dependencies
dotnet restore
4. Update Database
Make sure your appsettings.json has a valid SQL Server connection string, for example:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QuizPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
Then run:
dotnet ef database update
5. Run the Application
dotnet run
App will start at:
https://localhost:5001
_
🧑‍💻 Default Roles (Example)
Role	Username	Password
Admin / Professor	admin@example.com	Admin@123
Student	student@example.com	Student@123
(Update based on your seeded data.)
🧰 EF Core Commands (with explanations)
🪜 1. Add EF Tools (if not installed)
Run this once globally (so you can use dotnet ef commands from anywhere):
dotnet tool install --global dotnet-ef
✅ To confirm it’s installed:
dotnet ef –version

🏗️ 2. Add EF Core Packages (if not already installed)
In your case, these are already installed, but for reference:
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10

🧱 3. Create the Initial Migration
This command generates the SQL scripts (C# migration files) to create tables from your models:
dotnet ef migrations add InitialCreate
✅ You should see output like:
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
This creates a folder named Migrations/ with .cs files defining your database schema.
🗄️ 4. Apply the Migration (Create Database)
This command actually creates the database (QuizPortalDB) in your SQL Server using the migration:
dotnet ef database update
✅ You should see:
Applying migration '20251028123456_InitialCreate'.
Done.
Your database is now ready!

⚙️ 5. (Optional) Add New Migration After Model Change
If you later modify a model (for example, add a Marks property in Score.cs), you must create a new migration:
dotnet ef migrations add AddMarksToScore
Then apply it:
dotnet ef database update
________________________________________
🧹 6. (Optional) Remove Last Migration (if needed)
If you made a mistake while generating a migration:
dotnet ef migrations remove
________________________________________
📜 7. (Optional) View SQL Script of a Migration
To see the actual SQL that will run:
dotnet ef migrations script
________________________________________
🧩 Typical Full Setup (Quick Reference)
If your college just wants the quick commands:
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
________________________________________
🧾 Notes
Ensure SQL Server is running locally before launching.
If you face EF Core version issues, make sure all EF packages use version 8.0.10.
Migrations are already included in the project. If not, create with:
dotnet ef migrations add InitialCreate
dotnet ef database update
________________________________________

▶️ How to Run the Project
Option 1: Run via CLI
dotnet run
Option 2: Run via Visual Studio
•	Open QuizPortal.sln
•	Press Ctrl + F5 or click Run
________________________________________
🌐 Access the App
Once running, open your browser and go to:
http://localhost:5000
or
https://localhost:5001
________________________________________
👨‍🏫 Roles in the App
🧑‍🏫 Professor:
•	Create and manage quizzes
•	View all student results
🎓 Student:
•	Register or log in
•	Take quizzes
•	View their score after submission
________________________________________
📦 Folder Structure
QuizPortal/
│
├── Controllers/          → Handles application logic
├── Models/               → Database entities
├── Views/                → Razor views (UI)
├── Migrations/           → Auto-generated EF migration files
├── Data/                 → DbContext configuration
├── wwwroot/              → Static files (CSS, JS, Images)
└── appsettings.json      → Database connection string & config
________________________________________
🧠 Notes
•	If you modify models, run:
•	dotnet ef migrations add <MigrationName>
•	dotnet ef database update
•	If you see version conflicts between EF 8.x and 9.x, align all EF packages to 8.0.10.
________________________________________
⚠️ Common Errors & Fixes
🧩 EF Package Downgrade Warning
If you see:
Detected package downgrade: Microsoft.EntityFrameworkCore.Design from 9.x.x to 8.x.x
➡ Fix by aligning versions:
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
🗃️ Missing Columns or Migration Errors
If database schema changes:
dotnet ef migrations add UpdateSchema
dotnet ef database update
🔗 SQL Server Connection Issue
Check your appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=QuizPortalDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
________________________________________
🏁 Author
Developed by: Atharva Shelar
📧 your-23amtics404@gmail.com
💻 For Academic Use (College Project)
________________________________________

