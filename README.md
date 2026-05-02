# SkyRoute

A flight search & booking platform built with Clean Architecture.

## Tech Stack

### Backend
- .NET 10 (originally planned for .NET 9)
- ASP.NET Core Web API with Controllers
- Entity Framework Core with SQLite
- FluentValidation

### Frontend (To be implemented)
- Angular 18+ with standalone components

## Architecture

Clean Architecture with 4 projects:

### SkyRoute.Domain
Core business entities, value objects, and domain rules. No external dependencies.

**Structure:**
- `Entities/` - Domain entities
- `ValueObjects/` - Value objects
- `Enums/` - Domain enumerations

### SkyRoute.Application
Use cases, DTOs, interfaces, and application logic.

**Structure:**
- `DTOs/` - Data Transfer Objects
- `Interfaces/` - Application interfaces (repositories, services)
- `UseCases/` - Application use cases
- `Common/` - Result pattern and shared application logic

**Dependencies:** Domain

### SkyRoute.Infrastructure
Infrastructure concerns: database, external providers, pricing strategies.

**Structure:**
- `Persistence/` - EF Core DbContext, repositories
- `Providers/` - Mock flight providers (GlobalAir, BudgetWings)
- `PricingStrategies/` - Provider-specific pricing logic

**Dependencies:** Application, Domain

### SkyRoute.Api
API controllers, middleware, and dependency injection setup.

**Structure:**
- `Controllers/` - API endpoints
- `Middleware/` - Custom middleware

**Dependencies:** Application, Infrastructure

## Business Rules

### Flight Providers
- **GlobalAir**: base fare + 15% fuel surcharge, rounded to 2 decimals
- **BudgetWings**: base fare - 10% discount, minimum final price $29.99

### Document Requirements
- International flights (different origin/destination countries): Passport required
- Domestic flights (same country): National ID required

## Getting Started

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API
dotnet run --project src/SkyRoute.Api
```

## Project Status

Initial skeleton created. API contracts and business logic to be implemented.
