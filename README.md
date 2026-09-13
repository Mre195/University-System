# University Management System API

A role-based university management system built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**. It supports three roles — **Admin**, **Professor**, and **Student** — with features for managing departments, majors, courses, sections, enrollments, marks, attendance, notifications, and special requests.

## Tech Stack

- **Framework:** ASP.NET Core Web API
- **Target Framework:** `net10.0`
- **SDK:** .NET 10.0.400 (also compatible with 8.0.303 and 10.0.302)
- **Runtime:** ASP.NET Core 10.0.12 / .NET 10.0.12
- **ORM:** Entity Framework Core 10.0.11
- **Database:** SQL Server
- **Authentication:** JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.10)
- **Password Hashing:** BCrypt.Net-Next 4.2.0
- **JWT Tokens:** System.IdentityModel.Tokens.Jwt 8.22.0
- **API Testing:** Postman (Swashbuckle 10.2.3 is referenced, but Swagger UI is not configured in `Program.cs`)

## NuGet Packages (Top-Level)

**UniversitySystem.Api**
- Microsoft.AspNetCore.Authentication.JwtBearer — 10.0.10
- Microsoft.AspNetCore.OpenApi — 10.0.10
- Microsoft.OpenApi — 2.7.5
- Swashbuckle.AspNetCore — 10.2.3
- System.IdentityModel.Tokens.Jwt — 8.22.0

**UniversitySystem.Repository**
- Microsoft.EntityFrameworkCore.Design — 10.0.11
- Microsoft.EntityFrameworkCore.SqlServer — 10.0.11
- Microsoft.EntityFrameworkCore.Tools — 10.0.11

**UniversitySystem.Services**
- BCrypt.Net-Next — 4.2.0

## Solution Structure

```
UniversitySystem.Api          → Controllers, Program.cs, appsettings
UniversitySystem.Services     → DTOs, Services, Enums, Business Logic
UniversitySystem.Repository   → EF Core Models, DbContext, SQL Schema
```

## Roles & Features

### Admin
- Create and list departments
- Create and list majors
- Create and list semesters
- Create and list courses
- Create and list sections
- Create student accounts
- Create professor accounts
- List students and professors
- List special requests by status
- Review special requests (approve/reject)

### Professor
- View assigned sections (active semester only)
- View section marks
- Update student marks (assignment, mid, final)
- View section attendance
- Update section attendance
- View students in their department

### Student
- View current schedule
- View available sections for registration
- Register in a section
- View completed courses
- View semester marks
- View unread notification count
- View notifications
- Mark notification as read
- Submit special requests
- View own special requests

### Shared / Auth
- Login with email and password (JWT)
- Get profile based on role
- Hash password utility (for testing)

## Database Schema

The SQL script creates the following tables:

- Department
- Major
- User
- Admin
- Professor
- Student
- Course
- Semester
- Section
- EnrollmentStatus
- Enrollment
- SpecialRequestType
- SpecialRequestStatus
- SpecialRequest
- Attendance
- Notification

**Key constraints and defaults:**

- `Course.CreditHours` between 1 and 6
- `Major.Hours` between 120 and 263
- `Section.RoomCapacity` only 30 or 60
- `Semester.EndDate` must be after `StartDate`
- `Enrollment` unique per student and section
- `SpecialRequestStatusId` between 1 and 3
- Defaults for IDs, timestamps, and status flags

> **Note:** The SQL script does not include seed data for `EnrollmentStatus`, `SpecialRequestStatus`, `SpecialRequestType`, or an initial Admin user. You must insert these manually before using the API.

## API Endpoints

### General / Auth

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `api/login` | Anonymous | Login and receive JWT |
| GET | `api/profile` | Authorized | Get current user profile |
| POST | `api/hashed-password` | Anonymous | Hash a password (testing) |

### Admin

| Method | Route | Description |
|--------|-------|-------------|
| POST | `api/admin/create-department` | Create department |
| GET | `api/admin/get-all-departments` | List departments |
| POST | `api/admin/create-major` | Create major |
| GET | `api/admin/get-all-majors?departmentId=` | List majors by department |
| POST | `api/admin/create-semester` | Create semester |
| GET | `api/admin/get-all-semesters` | List semesters |
| POST | `api/admin/create-course` | Create course |
| GET | `api/admin/get-all-courses?majorId=` | List courses by major |
| POST | `api/admin/create-section` | Create section |
| GET | `api/admin/get-all-sections?semesterId=&courseId=` | List sections |
| POST | `api/admin/create-student` | Create student account |
| GET | `api/admin/get-all-students` | List students |
| POST | `api/admin/create-professor` | Create professor account |
| GET | `api/admin/get-all-professors` | List professors |
| GET | `api/admin/get-all-special-requests?statusId=` | List special requests by status |
| GET | `api/admin/special-requests/review` | Review special request |

### Professor

| Method | Route | Description |
|--------|-------|-------------|
| GET | `api/professors/courses` | Get professor sections |
| GET | `api/professors/sections/{sectionId}/marks` | Get section marks |
| POST | `api/professors/marks` | Update marks |
| GET | `api/professors/sections/{sectionId}/attendance` | Get attendance |
| POST | `api/professors/sections/attendance` | Update attendance |
| GET | `api/professors/department-students` | Get department students |

### Student

| Method | Route | Description |
|--------|-------|-------------|
| GET | `api/student/my-schedule` | Get schedule |
| GET | `api/student/available-sections` | Get available sections |
| POST | `api/student/section-registration` | Register in section |
| GET | `api/student/completed-courses` | Get completed courses |
| GET | `api/student/semester-marks` | Get semester marks |
| GET | `api/student/notifications-unread-count` | Unread count |
| GET | `api/student/notifications` | Get notifications |
| PUT | `api/student/notifications/{notificationId}/read` | Mark as read |
| POST | `api/student/special-requests` | Submit special request |
| GET | `api/student/my-special-requests` | Get own requests |
| GET | `api/student/review-my-special-requests` | Review own request status |

## Authentication & Authorization

- JWT Bearer is used for authentication.
- Tokens are generated in `JwtService` with claims: `NameIdentifier`, `Email`, `Role`.
- Token expiry: 1 hour.
- Role is determined during login by checking if the user is a Student, Professor, or Admin.
- `[Authorize(Roles = "...")]` is applied to controllers.

## Configuration

In `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Uni_App_Db;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32BytesLong!",
    "Issuer": "Owner",
    "Audience": "University_Users"
  }
}
```

## Business Rules

- **Course:** Credit hours between 1 and 6; unique name.
- **Major:** Hours between 120 and 263; unique name.
- **Department:** Unique name; optional head professor must exist.
- **Section:** Room capacity must be 30 or 60; no professor schedule clash.
- **Enrollment:**
  - Section cannot exceed room capacity.
  - Student cannot take the same course twice (registered or completed).
  - Student cannot have two sections at the same time slot.
  - Maximum 18 registered credit hours.
- **Marks:** Total = assignment + mid + final; letter grade A/B/C/D/F.
- **Attendance:** One record per enrollment per day; update or insert.
- **Semester:** Only one active semester at a time; auto-updated by date.
- **Special Requests:** Pending → Approved/Rejected; generates notification.

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or remote)
- Postman (recommended)

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd "University System"
```

### 2. Create the database

Run the provided SQL script against your SQL Server instance to create the schema.

### 3. Seed required lookup data

Insert rows into:

- `EnrollmentStatus` (1=Registered, 2=Withdrawn, 3=Failed, 4=Completed)
- `SpecialRequestStatus` (1=Pending, 2=Approved, 3=Rejected)
- `SpecialRequestType` (1=GradeAppeal, 2=AddDrop, 3=LeaveOfAbsence, 4=CourseOverride, 5=Other)
- `User` + `Admin` for the first admin account.

### 4. Configure connection string and JWT

Edit `appsettings.Development.json` with your SQL Server connection and a secure JWT key.

### 5. Build and run

```bash
dotnet restore
dotnet build
dotnet run --project UniversitySystem.Api
```

### 6. Test with Postman

- Use `POST /api/login` to get a token.
- Add `Authorization: Bearer <token>` to subsequent requests.
- Test endpoints by role.

> Swagger/Swashbuckle packages are referenced, but Swagger UI is not enabled in `Program.cs`. If you want Swagger, add `AddSwaggerGen()` and `UseSwagger()`/`UseSwaggerUI()`.

## Project Structure

```text
UniversitySystem/
│
├── UniversitySystem.Api/
│   ├── Controllers/
│   │   ├── AdminController.cs
│   │   ├── ProfessorController.cs
│   │   ├── StudentController.cs
│   │   └── GeneralController.cs
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── UniversitySystem.Services/
│   ├── DTOs/
│   │   ├── Admin/
│   │   ├── Professor/
│   │   ├── Student/
│   │   └── Shared/
│   ├── Enums/
│   │   ├── EnrollmentStatusEnum.cs
│   │   ├── SpecialRequestStatusEnum.cs
│   │   └── SpecialRequestTypeEnum.cs
│   └── Services/
│       ├── Admin/
│       ├── Professor/
│       ├── Student/
│       └── Shared/
│
├── UniversitySystem.Repository/
│   ├── Context/
│   │   └── UniAppDbContext.cs
│   └── Models/
│       ├── Department.cs
│       ├── Major.cs
│       ├── User.cs
│       ├── Admin.cs
│       ├── Professor.cs
│       ├── Student.cs
│       ├── Course.cs
│       ├── Semester.cs
│       ├── Section.cs
│       ├── EnrollmentStatus.cs
│       ├── Enrollment.cs
│       ├── SpecialRequestType.cs
│       ├── SpecialRequestStatus.cs
│       ├── SpecialRequest.cs
│       ├── Attendance.cs
│       └── Notification.cs
│
└── UniversitySystem.sln
```

### Key Files

- `Program.cs` — DI, JWT, DbContext, service registration
- `UniAppDbContext.cs` — EF Core mappings
- `AdminController.cs`, `ProfessorController.cs`, `StudentController.cs`, `GeneralController.cs`
- `Services/Admin`, `Services/Professor`, `Services/Student`, `Services/Shared`
- `DTOs/Admin`, `DTOs/Professor`, `DTOs/Student`, `DTOs/Shared`
- `Models/` — EF Core entities
- `Enums/` — EnrollmentStatusEnum, SpecialRequestStatusEnum, SpecialRequestTypeEnum

## Known Limitations / TODO

- Update and Delete operations for Departments and Majors are commented out.
- No seed data included for lookup tables or initial Admin user.
- Swagger UI is not configured.
- Exception handling and request logging middleware are commented out in `Program.cs`.
- No pagination on list endpoints.
- No unit/integration tests included.

## License

This project is for educational purposes.
