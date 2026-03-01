# Fitness Tracker API

A REST API for tracking workouts. Built with ASP.NET Core (.NET 10) and PostgreSQL.

## Stack

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core + Npgsql (PostgreSQL)
- JWT authentication with refresh tokens
- AutoMapper
- FluentValidation
- Serilog (logs to console and file)
- Swagger (API docs)
- BCrypt for password hashing
- MailKit for sending emails

## Requirements

- .NET 10 SDK
- PostgreSQL
- (optional) SMTP server for email confirmation

## Configuration

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=fitness_tracker;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your_secret_key",
    "Issuer": "FitnessTracker",
    "Audience": "FitnessTracker"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "@gmail.com",
    "SenderPassword": "",
    "SkipEmailSending": false
  }
}
```

To skip email sending during development, set `SkipEmailSending: true`.

## Running

```bash
# Apply migrations
dotnet ef database update --project main

# Start the server
dotnet run --project main
```

Swagger docs will be available at: `http://localhost:5291/swagger`

## Project Structure

```
proj/
  main/         — main API project
    Controllers/    — controllers
    Services/       — business logic
    Repositories/   — database access
    Models/         — data models
    DTOs/           — data transfer objects
    Validators/     — request validation
    Migrations/     — EF Core migrations
    Middleware/     — error handling, email confirmation
    Data/           — DB context, seed data
  test/         — test project
  client/       — simple HTML client for manual testing
```

## API

### Authentication — `/api/auth`

POST  `/login` Login, returns access and refresh tokens
POST  `/register` Register a new user
POST  `/refresh` Get a new access token using a refresh token
POST  `/logout` Logout and invalidate the refresh token
GET `/all` Get all users (paginated)

### Users — `/api/user`

Manage user profile.

### Exercises — `/api/exercise`

Get and manage exercises. Supports filtering by muscle group.

### Standard Programs — `/api/standardprogram`

Official workout programs created by authors. Filterable by category, level, and workout type.

### Custom Programs — `/api/customprogram`, `/api/customprogramv2`

User-created programs. Can be public or private.

### Workout Sessions — `/api/workoutsession`

Records of a user's workouts. Each session is linked to a standard or custom program and has a status: `InProgress`, `Completed`, or `Cancelled`.

### Workout Exercise Sets — `/api/workoutexerciseset`

Individual sets (exercise + weight + reps) within a workout session.

## Data Models

**User** — name, email, author role, email confirmation status.

**Exercise** — title, sets, reps, muscle group (Chest / Back / Legs).

**StandardProgram** — official program with category, level, and workout type.

**CustomProgram** — user-created program, can be public or private.

**WorkoutSession** — a workout record with date and status.

**WorkoutExerciseSet** — a single set within a session.

## Tests

```bash
dotnet test
```

Tests are in the `test/` project and cover the service layer: authentication, exercises, programs, sessions, and email.

## Logs

Logs are written to the `logs/` folder, rotated daily, kept for 14 days. File format: `app-YYYYMMDD.log`.
