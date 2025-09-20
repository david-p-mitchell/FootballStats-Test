
# FootballStats-Test

An ASP.NET Core web application for managing and displaying football statistics.
This project is a testbed for experimenting with player management, stats tracking, and UI rendering.

---

## Features
- Player listing and stats
- Match results and upcoming fixtures
- Interactive frontend with HTML/JS/CSS + DataTables
- ASP.NET Core MVC + Web API backend
- Integration with Football-Data.org API
- HTTPS support for local development and production
- MIT licensed

---

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- SQL Server or Azure SQL
- Valid Football-Data.org API key
- Git

---

## Setup & Run

### 1. Clone the repository
```bash
git clone https://github.com/david-p-mitchell/FootballStats-Test.git
cd FootballStats-Test/AFCStatsApp
```

### 2. Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### 3. Configure database connection and football-data.org base url
Edit appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FootballStatsDb;Trusted_Connection=True;"
  },
  "FootballDataOrgApi": {
    "BaseUrl": "https://api.football-data.org/v4/",
    "ApiKey": ""
  }
}
```

### 4. Set secrets / API key
```bash
dotnet user-secrets init
dotnet user-secrets set "FootballDataOrgApi:ApiKey" "YOUR_API_KEY_HERE"
```

---

## EF Core Migrations

### Fresh Database
1. Create the initial migration:

```bash
dotnet ef migrations add InitialCreate
```

2. Apply the migration to create database and tables:

```bash
dotnet ef database update
```

### Existing Database / Preserve Data
1. Backup your database.
2. Create a baseline migration ignoring existing tables:

```bash
dotnet ef migrations add InitialCreate --ignore-changes
```
3. Apply the migration

```bash
dotnet ef database update
```
---
##Setup HTTPS
Check is https has a cert.
Then trust it.
```bash
dotnet dev-certs https --check

dotnet dev-certs https --trust

```

## Run the app
```bash
dotnet restore
dotnet build
dotnet run
```

- Open browser: https://localhost:7267 (if you have made the dev -cert trusted?)
Also on: http://localhost:5132

## License
MIT License
