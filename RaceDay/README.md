# RaceDay — Part 1

## Project Description

RaceDay is a full-stack web-based event management system for South African road running, walking and cycling events.

Part 1 focuses on planning the system, designing the database and planning the REST API.

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

## Part 1 Deliverables

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

## CI/CD

GitHub Actions validates that the required Part 1 files exist in the repository.

Before final submission, push this repository to GitHub and confirm the workflow completes successfully with a green check.

## Video Presentation

YouTube link: **TO BE ADDED BY STUDENT**

The Part 1 video should demonstrate:
- The ERD and design decisions
- The API endpoint plan
- The SQL script being executed in SQL Server
- The resulting database and tables

## AI Disclosure

AI tools were used as a supporting tool during planning, explanation and review. The student must review, understand, test and adapt the work before submission and must follow the institution's academic-integrity and AI-disclosure requirements.

## Important Final-Submission Items

The following cannot be truthfully completed inside a generated ZIP:
- The student's own GitHub repository and its actual commit history
- A real green GitHub Actions screenshot
- The student's unlisted YouTube presentation link
- Proof that the SQL script has been executed successfully in the student's assigned SQL Server environment

These must be completed by the student before submission.
