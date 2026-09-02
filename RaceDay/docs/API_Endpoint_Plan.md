# RaceDay API Endpoint Plan — Part 1

Base route: `/api`

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | `/api/auth/register` | Register a new participant account | Public | FirstName, LastName, Email, Password | 201 Created + user details |
| POST | `/api/auth/login` | Authenticate a user | Public | Email, Password | 200 OK + authentication token |
| GET | `/api/users/me` | View logged-in user's profile | Authenticated | None | 200 OK + profile |
| PUT | `/api/users/me` | Update logged-in user's profile | Authenticated | FirstName, LastName, Email | 200 OK + updated profile |
| GET | `/api/users/{id}` | View a user profile | Organiser | None | 200 OK + user details |
| GET | `/api/events` | Browse events | Public | None | 200 OK + event list |
| GET | `/api/events/{id}` | View event details | Public | None | 200 OK + event |
| POST | `/api/events` | Create an event | Organiser | Event details | 201 Created + event |
| PUT | `/api/events/{id}` | Edit an event | Organiser | Event details | 200 OK + updated event |
| DELETE | `/api/events/{id}` | Delete an event | Organiser | None | 204 No Content |
| GET | `/api/events/{eventId}/categories` | View categories for an event | Public | None | 200 OK + category list |
| GET | `/api/categories/{id}` | View one category | Public | None | 200 OK + category |
| POST | `/api/events/{eventId}/categories` | Create an event category | Organiser | CategoryName, EntryFee, MaxParticipants | 201 Created + category |
| PUT | `/api/categories/{id}` | Edit a category | Organiser | Category details | 200 OK + updated category |
| DELETE | `/api/categories/{id}` | Delete a category | Organiser | None | 204 No Content |
| POST | `/api/categories/{categoryId}/enrolments` | Enrol the current participant | Participant | None or enrolment details | 201 Created + enrolment |
| GET | `/api/enrolments/me` | View own enrolments | Participant | None | 200 OK + enrolment list |
| GET | `/api/events/{eventId}/enrolments` | View all enrolments for an event | Organiser | None | 200 OK + enrolment list |
| GET | `/api/enrolments/{id}` | View an enrolment | Participant / Organiser | None | 200 OK + enrolment |
| DELETE | `/api/enrolments/{id}` | Cancel an enrolment | Participant | None | 204 No Content |
| POST | `/api/enrolments/{enrolmentId}/results` | Capture a participant result | Organiser | FinishTime, Position, ResultStatus | 201 Created + result |
| GET | `/api/results/me` | View own results | Participant | None | 200 OK + result list |
| GET | `/api/events/{eventId}/results` | View event results | Organiser | None | 200 OK + result list |
| GET | `/api/results/{id}` | View one result | Participant / Organiser | None | 200 OK + result |
| PUT | `/api/results/{id}` | Update a result | Organiser | FinishTime, Position, ResultStatus | 200 OK + updated result |
| DELETE | `/api/results/{id}` | Delete a result | Organiser | None | 204 No Content |

## Notes

- Authentication should use secure password hashing in Part 2; the Part 1 SQL seed values are placeholders only.
- Organiser-only operations must verify that the authenticated user has the Organiser role.
- Participant-only operations must verify that the authenticated user has the Participant role.
- API validation should enforce ownership, valid event/category relationships, and duplicate-enrolment prevention.
- HTTP 400 should be used for invalid input, 401 for unauthenticated requests, 403 for insufficient permissions, 404 for missing resources, and 409 for conflicts such as duplicate enrolments.
