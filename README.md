
# 🎓 Online Education Platform

This is a collaborative ASP.NET Core MVC project implementing an **Online Education Platform** using **Clean Architecture**, **Identity for authentication**, **Bootstrap 5 for UI**, and **Entity Framework Core** for data access.

---

## 🧱 Project Structure (Clean Architecture)

```
OnlineEducationPlatform/
│
├── OnlineEducationPlatform.Web           → Presentation layer (MVC)
├── OnlineEducationPlatform.Application   → Business logic & use cases
├── OnlineEducationPlatform.Infrastructure→ Data access & Identity
├── OnlineEducationPlatform.Domain        → Core domain models
└── OnlineEducationPlatform.sln           → Solution file
```

---

## 🛠️ Getting Started

Follow these steps to set up the project locally:

### ✅ 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/OnlineEducationPlatform.git
cd OnlineEducationPlatform
```

### ✅ 2. Restore Packages

```bash
dotnet restore
```

### ✅ 3. Set Up Local Database

Ensure you have **SQL Server LocalDB** or equivalent running. Then:

- Update the connection string in `OnlineEducationPlatform.Web/appsettings.Development.json` (create this file if missing):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OnlineEducationDb_YourName;Trusted_Connection=True;"
  }
}
```

> 🛡️ Each developer should use a **unique local DB name** to avoid conflicts.

---

### ✅ 4. Apply Migrations & Initialize the DB

```bash
dotnet ef database update --project OnlineEducationPlatform.Infrastructure --startup-project OnlineEducationPlatform.Web
```

---

### ✅ 5. Run the App

```bash
dotnet run --project OnlineEducationPlatform.Web
```

Then open your browser at:

```
https://localhost:5001
```

---

## 👥 User Roles

The platform includes role-based access with the following roles:

- **Admin** – manages users (instructors, students)
- **Instructor**
- **Student**

Only Admin can:

- Register users
- Assign roles
- View/manage users (excluding other admins)

---

## 🧪 Development Features

- 👤 Authentication via **ASP.NET Core Identity**
- 📂 MVC structure with layout & partial views
- 🧼 **Bootstrap 5** for responsive UI
- 🔐 Admin login restricted to known credentials
- 🚫 Admin cannot see or manage other Admins

---

## 📦 Tools Used

- [.NET 9](https://dotnet.microsoft.com/)
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Identity
- SQL Server / LocalDB
- Bootstrap 5

---

## 🤝 Collaborating

1. Always **create a branch** for new features:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. After changes:
   ```bash
   git add .
   git commit -m "Added XYZ"
   git push origin feature/your-feature-name
   ```

3. Submit a pull request via GitHub.

---

## 📋 EF Core Migrations (For Developers)

> Only run this if you're adding/changing models.

```bash
dotnet ef migrations add YourMigrationName --project OnlineEducationPlatform.Infrastructure --startup-project OnlineEducationPlatform.Web
dotnet ef database update --project OnlineEducationPlatform.Infrastructure --startup-project OnlineEducationPlatform.Web
```

---

## 🔐 Secrets (Optional)

Use `dotnet user-secrets` to store development secrets securely:

```bash
dotnet user-secrets init
dotnet user-secrets set "AdminPassword" "Test123!"
```

---

## 📧 Contact

For questions or contributions, please open an issue or contact a project maintainer.

---

### 🚀 Happy coding!
