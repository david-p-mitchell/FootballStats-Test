# FootballStats-Test

An ASP.NET Core web application for managing and displaying football statistics.  
This project is a testbed for experimenting with player management, stats tracking, and UI rendering.

---

##  Features
- Player listing and stats
- Match results listing and next fixtures listing
- Interactive frontend (HTML/JS/CSS + DataTables)
- Backend built with ASP.NET Core (C#)
- MIT licensed

---

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- SQL Server or Azure SQL

---

## Setup & Run

### 1. Clone the repository
```bash
git clone https://github.com/david-p-mitchell/FootballStats-Test.git
cd FootballStats-Test/AFCStatsApp
```

### 2. Configure App Settings and Secrets
Secrets shouldn't be hardcoded and stored in app.settings.json.
Put the 
```json
"FootballDataOrgApi": {
  "BaseUrl": "https://api.football-data.org/v4/",
  "ApiKey": ""
}
```

Instead set the API Football Data API Key and Database Connection using .Net user secrets.
```bash
cd AFCStatsApp
dotnet user-secrets init
dotnet user-secrets set "FootballDataOrgApi:ApiKey" "YOUR_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:FootballDb" "YOUR_CONNECTION_STRING"


```


### 3. Run the Project
```bash
cd FootballStatsTest
dotnet restore
dotnet build
dotnet run
```

The App will appear at localhost:5001;

