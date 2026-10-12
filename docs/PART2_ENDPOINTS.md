# Part 2 API Endpoint Overview

| Method | Route | Access | Purpose |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Register Organiser/Participant |
| POST | `/api/auth/login` | Public | Login and establish cookie/session |
| POST | `/api/auth/logout` | Current session | Logout |
| GET/PUT | `/api/users/me` | Authenticated | View/update own profile |
| GET | `/api/users/{id}` | Organiser | View user profile |
| GET | `/api/events` | Public | List events |
| GET | `/api/events/{id}` | Public | Get event |
| POST/PUT/DELETE | `/api/events[/{id}]` | Organiser; owner for update/delete | Manage events |
| GET | `/api/events/{eventId}/categories` | Public | List categories |
| GET | `/api/categories/{id}` | Public | Get category |
| POST | `/api/events/{eventId}/categories` | Organiser; owner only | Create category |
| PUT/DELETE | `/api/categories/{id}` | Organiser; owner only | Update/delete category |
| POST | `/api/categories/{categoryId}/enrolments` | Participant | Enrol |
| GET | `/api/enrolments/me` | Participant | Own enrolments |
| GET | `/api/events/{eventId}/enrolments` | Organiser; owner only | View event enrolments |
| GET/DELETE | `/api/enrolments/{id}` | Owner/organiser; participant for cancellation | View/cancel enrolment |
| POST | `/api/enrolments/{enrolmentId}/results` | Organiser; owner only | Record result |
| GET | `/api/results/me` | Participant | Own results |
| GET | `/api/events/{eventId}/results` | Organiser; owner only | Event results |
| GET/PUT/DELETE | `/api/results/{id}` | Owner/organiser; Organiser for mutation | View/manage result |

Expected status codes include 200, 201, 204, 400, 401, 403, 404 and 409. Swagger documents the endpoints. Test the actual routes and role rules before submission.
