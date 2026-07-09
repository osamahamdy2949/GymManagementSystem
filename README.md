# Gym Management System

A full-stack ASP.NET Core MVC application for managing gym members, trainers, plans, memberships, sessions, bookings, attendance, and dashboard analytics.

The system is built with a layered architecture using MVC, Entity Framework Core, ASP.NET Core Identity, repository/unit-of-work patterns, and service-based business logic.

## Features

- Secure login with ASP.NET Core Identity
- Role-based authorization
- Dashboard analytics for members, trainers, and sessions
- Member management with profile photos and health records
- Trainer management
- Membership plan management
- Membership creation and cancellation
- Training session scheduling
- Session booking management
- Attendance tracking for ongoing sessions
- SQL Server database with Entity Framework Core migrations
- Automatic database migration and seed data on application startup

## Tech Stack

- ASP.NET Core MVC
- .NET 9
- Entity Framework Core 9
- SQL Server
- ASP.NET Core Identity
- AutoMapper
- Repository Pattern
- Unit of Work Pattern
- Razor Views
- Bootstrap
- jQuery Validation

## Project Structure

```text
GymManagement/
├── GymManagement/          # Presentation Layer - MVC controllers, views, wwwroot
├── GymManagement.BLL/      # Business Logic Layer - services, view models, mapping
├── GymManagement.DAL/      # Data Access Layer - DbContext, models, repositories, migrations
└── GymManagementSystem.slnx
```

## Architecture

The project follows a clean layered architecture:

- **Presentation Layer**: Handles MVC controllers, Razor views, authentication flow, and UI assets.
- **Business Logic Layer**: Contains application services, view models, validation flow, and AutoMapper profiles.
- **Data Access Layer**: Contains database models, EF Core configurations, repositories, migrations, and data seeding.

## Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET 9 SDK
- SQL Server
- Visual Studio 2022 or later

## Installation

1. Clone the repository:

```bash
git clone https://github.com/osamahamdy2949/G-Net-34-MVC01.git
cd G-Net-34-MVC01
```

2. Update the database connection string in:

```text
GymManagement/appsettings.Development.json
```

Default connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Build the solution:

```bash
dotnet build GymManagementSystem.slnx
```

4. Run the application:

```bash
dotnet run --project GymManagement/GymManagement.PL.csproj
```

5. Open the application in your browser:

```text
https://localhost:7130
```

or:

```text
http://localhost:5209
```

## Database Setup

The application automatically applies pending EF Core migrations and seeds initial data when it starts.

Seeded data includes:

- Membership plans from `wwwroot/Files/plans.json`
- Initial identity roles and users

## Demo Login

Use the seeded Super Admin account for development testing:

```text
Email: osamaelnaqeeb@gym.com
Password: P@ssw0rd
```

> Important: Change seeded credentials before using the application in production.

## Main Modules

### Members

- Add, update, delete, and view gym members
- Upload and display member photos
- Store member health records
- View member details and health information

### Trainers

- Add, update, delete, and view trainers
- Store trainer contact, address, and speciality information

### Plans

- View membership plans
- Edit plan details
- Activate or deactivate plans

### Memberships

- Create memberships for members
- Link members to available plans
- Cancel active memberships

### Sessions

- Create and manage training sessions
- Assign trainers and categories
- Track upcoming, ongoing, and completed sessions

### Bookings

- Book members into sessions
- View members registered for sessions
- Cancel bookings
- Mark attendance for ongoing sessions

### Analytics

The dashboard displays:

- Total members
- Active members
- Total trainers
- Upcoming sessions
- Ongoing sessions
- Completed sessions

## Authorization

The system uses ASP.NET Core Identity.

Some modules require authenticated access, while sensitive management features such as members and trainers are restricted to the `SuperAdmin` role.

## Useful Commands

Add a new migration:

```bash
dotnet ef migrations add MigrationName --project GymManagement.DAL --startup-project GymManagement
```

Update the database manually:

```bash
dotnet ef database update --project GymManagement.DAL --startup-project GymManagement
```

Run the application:

```bash
dotnet run --project GymManagement
```

## Future Enhancements

- Add user registration and admin user management
- Add payment tracking for memberships
- Add reporting and export features
- Add email notifications for bookings and memberships
- Add automated tests
- Improve role and permission management
- Add API endpoints for mobile or frontend clients

## License

This project is currently not licensed. Add a license file before publishing or distributing the project.

## Author

Developed by Osama Hamdy.
