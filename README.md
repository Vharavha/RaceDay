# RaceDay

RaceDay is a full-stack web-based event management system for South African road running, walking and cycling events.

- **Part 1** — system planning, ERD, SQL Server database and REST API endpoint plan.
- **Part 2** — RESTful API (ASP.NET Core Web API, .NET 8, EF Core, SQL Server).

## Repository Structure

```
RaceDay/
├── README.md
├── .gitignore
├── .github/workflows/
│   ├── part1-validation.yml      # Part 1 repository-structure check
│   └── part2-api-ci.yml          # Part 2 build and test
├── docs/
│   ├── RaceDay_ERD.png
│   ├── RaceDay_Database.sql
│   ├── API_Endpoint_Plan.md
│   ├── PART1_FINAL_CHECKLIST.md
│   └── PART2_ENDPOINTS.md
└── Part2/
    ├── RaceDay.Api/              # Web API project
    └── RaceDay.Tests/            # xUnit tests
```

## User Roles

### Organiser
- Create, edit and delete events
- Manage event categories
- View event enrolments
- Capture and manage participant results

### Participant
- Create an account and log in
- Browse events and categories
- Enrol in an event category
- View personal enrolments
- Track personal results

---

# Part 1 — Planning and Database Design

## Deliverables

- `docs/RaceDay_ERD.png` — ERD with entities, attributes, PKs, FKs and cardinalities.
- `docs/API_Endpoint_Plan.md` — REST API endpoint plan.
- `docs/RaceDay_Database.sql` — SQL Server database creation and seed script.
- `.github/workflows/part1-validation.yml` — GitHub Actions repository-structure validation.
- `README.md` — project overview and documentation.

## Database

The SQL script creates the following entities:

1. Users
2. Events
3. Categories
4. Enrolments
5. Results

It includes primary keys, foreign keys, NOT NULL constraints, UNIQUE constraints, DEFAULT values, CHECK constraints and realistic sample data.

## How to Test the SQL

Open the SQL script in SQL Server Management Studio (SSMS) or another SQL Server-compatible environment, connect to a SQL Server instance, and execute the complete script.

The script creates `RaceDayDB` and displays all five tables after the inserts.

## CI/CD (Part 1)

GitHub Actions validates that the required Part 1 files exist in the repository.

## Part 1 Video Presentation

YouTube link: **TO BE ADDED BY STUDENT**

The Part 1 video demonstrates:
- The ERD and design decisions
- The API endpoint plan
- The SQL script being executed in SQL Server
- The resulting database and tables

---

# Part 2 — RESTful API

ASP.NET Core Web API (.NET 8), EF Core, SQL Server, cookie authentication + session, role-based authorisation, Swagger and xUnit integration tests.

## Before running
1. Install the .NET 8 SDK and SQL Server.
2. Edit `Part2/RaceDay.Api/appsettings.json` so `DefaultConnection` matches your SQL Server.
3. From the repository root run:

```bash
dotnet restore Part2/RaceDay.Tests/RaceDay.Tests.csproj
dotnet build Part2/RaceDay.Api/RaceDay.Api.csproj
dotnet test Part2/RaceDay.Tests/RaceDay.Tests.csproj
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project Part2/RaceDay.Api --startup-project Part2/RaceDay.Api
dotnet ef database update --project Part2/RaceDay.Api --startup-project Part2/RaceDay.Api
dotnet run --project Part2/RaceDay.Api
```

Open `/swagger` on the URL shown in the terminal. Do not run `dotnet tool install` if `dotnet-ef` is already installed.

## Features
- Register/login/logout; ASP.NET Core password hashing.
- HTTP-only authentication cookie and server-side session state.
- Organiser and Participant roles.
- Events, categories, enrolments, results and user profile endpoints.
- Swagger and GitHub Actions build/test workflow.

See `docs/PART2_ENDPOINTS.md` for the endpoint list.

## Notes
The API uses the five Part 1 entities. `EventType` (Run, Walk or Cycle) was added to Events in Part 2. The default connection string uses LocalDB; replace it if your environment uses another SQL Server instance.

Public registration allows either role to match the brief. For a real production system, Organiser accounts should normally require administrator approval.

## Successful CI screenshot (Part 2)
Replace this text with your own screenshot of a successful GitHub Actions run.

## Part 2 video
Paste your unlisted YouTube URL here.

---

## AI Disclosure

AI tools were used as a supporting tool during planning, scaffolding, explanation and review of both parts of this project. The student has reviewed, tested and adapted the work and discloses this assistance in accordance with the institution's academic-integrity and AI-disclosure requirements.
