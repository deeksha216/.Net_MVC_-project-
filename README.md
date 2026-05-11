# HabitTracker — ASP.NET Core MVC + SQL Server
**Deeksha D | Nitte Institute of Professional Education | 2026**

---

## How to Run (VS Code)

### Step 1 — Find your SQL Server name
Open VS Code Terminal (Ctrl + `) and run:
```
sqlcmd -L
```
Note the server name printed (e.g. DESKTOP-ABC\SQLEXPRESS)

### Step 2 — Update appsettings.json
Open `appsettings.json` and update the Server value:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=HabitTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Common server names:
| Your SQL Server type     | Server value              |
|--------------------------|---------------------------|
| SQL Server Express       | .\\SQLEXPRESS             |
| SQL Server Developer     | localhost                 |
| LocalDB (with VS)        | (localdb)\\MSSQLLocalDB   |
| Named instance           | DESKTOP-ABC\\SQLEXPRESS   |

### Step 3 — Install EF Tools (one time only)
```
dotnet tool install --global dotnet-ef
```

### Step 4 — Create the database
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 5 — Run
```
dotnet run
```
Open https://localhost:5001 in your browser.

---

## Tech Stack
- **Frontend**: HTML5, CSS3, Bootstrap 5, Bootstrap Icons, Chart.js
- **Backend**: ASP.NET Core 8 MVC, C#
- **Database**: Microsoft SQL Server (Entity Framework Core)
- **Auth**: ASP.NET Core Identity

## Features
- Register & Login
- Create, Edit, Delete habits
- Mark habits complete / undo
- Current & longest streak calculation
- Dashboard with stats
- Statistics page: bar chart, donut, heatmap, per-habit progress
- Responsive design (mobile + desktop)
