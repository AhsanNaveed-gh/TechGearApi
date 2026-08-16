# TechGearAPI

TechGearAPI is a backend REST API built with **ASP.NET Core**, **Entity Framework Core**, and **PostgreSQL**. The project demonstrates modern backend development practices including layered architecture, dependency injection, JWT authentication, role-based authorization, secure password storage, middleware, DTOs, and Docker containerization.

---

## Features

- User Registration & Login
- JWT Authentication
- BCrypt Password Hashing
- Role-Based Authorization (Admin & Customer)
- Product CRUD Operations
- Product Search
- Product Sorting
- Pagination
- Global Exception Handling Middleware
- Request Logging Middleware
- Entity Framework Core with PostgreSQL
- DTO-based Request/Response Models
- Dependency Injection
- Service Layer Architecture
- Database Migrations
- Docker Containerization

---

## Tech Stack

- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT Bearer Authentication**
- **BCrypt.Net**
- **Docker**
- **Docker Desktop**
- **WSL2**
- **Visual Studio 2022**
- **Postman**

---

## Project Structure

```text
TechGearAPI/
│
├── Controllers/
│   ├── ProductController.cs
│   ├── UserController.cs
│   └── WeatherForecastController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── DTOs/
│
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
│
├── Migrations/
│
├── Models/
│
├── Services/
│
├── Properties/
│
├── .dockerignore
├── .gitignore
├── Dockerfile
├── Program.cs
├── appsettings.json
├── appsettings.example.json
└── TechGearAPI.csproj


API Endpoints
Register
POST /api/User/register
Content-Type: application/json

Example Request
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "Password123!"
}

Login
POST /api/User/login
Content-Type: application/json

Example Request
{
  "email": "john@example.com",
  "password": "Password123!"
}
The login endpoint returns a JWT token.
Use the token for protected endpoints:
Authorization: Bearer <JWT_TOKEN>

All product endpoints require authentication unless otherwise configured.

| Method   | Endpoint                               | Authorization |
| -------- | -------------------------------------- | ------------- |
| `GET`    | `/api/Product`                         | Authenticated |
| `GET`    | `/api/Product/{id}`                    | Authenticated |
| `POST`   | `/api/Product`                         | Admin         |
| `PUT`    | `/api/Product/{id}`                    | Admin         |
| `DELETE` | `/api/Product/{id}`                    | Admin         |
| `GET`    | `/api/Product/search?term=`            | Authenticated |
| `GET`    | `/api/Product/sort?by=&order=`         | Authenticated |
| `GET`    | `/api/Product/paged?page=1&pageSize=5` | Authenticated |


Authentication & Authorization

TechGearAPI uses JWT Bearer Authentication.

The authentication flow is:

User Registration
       ↓
BCrypt Password Hashing
       ↓
PostgreSQL
       ↓
User Login
       ↓
JWT Token Generated
       ↓
Authorization: Bearer <JWT>
       ↓
Protected API Endpoint

Role-based authorization is implemented for:

Admin
Customer

Product creation, updating, and deletion require the Admin role.

**Database**

The project uses:

PostgreSQL
Entity Framework Core
EF Core Migrations

The application uses a PostgreSQL database named:

TechGearDb

Database credentials should not be committed to the repository.


**Configuration**

Sensitive configuration values are kept outside the Git repository.

Use:

appsettings.example.json

as a template for the required configuration.

Example:
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=host.docker.internal;Port=5432;Database=TechGearDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "TechGearAPI",
    "Audience": "TechGearAPI"
  }
}

Running Locally
1. Clone the repository
git clone https://github.com/AhsanNaveed-gh/TechGearApi.git
cd TechGearApi
2. Configure PostgreSQL

Make sure PostgreSQL is installed and running.

Create/configure the TechGearDb database and provide your local PostgreSQL credentials through your application configuration.

Use appsettings.example.json as a reference.

3. Apply EF Core migrations
dotnet ef database update
4. Run the API
dotnet run

The API will start using the configured ASP.NET Core environment.

Running with Docker

TechGearAPI can also be run inside a Docker container.

Prerequisites

Install:

Docker Desktop
WSL2
PostgreSQL

Make sure Docker Desktop is running before executing the Docker commands.

Build the Docker Image

From the project directory:

docker build -t techgearapi .
Run the Container

Configuration values are supplied at runtime rather than being stored inside the Docker image.

PowerShell example:

docker run -d -p 8080:8080 `
  --name techgearapi-container `
  -e "Jwt__Key=YOUR_JWT_SECRET" `
  -e "ConnectionStrings__DefaultConnection=YOUR_CONNECTION_STRING" `
  techgearapi

The API will then be available at:

http://localhost:8080
PostgreSQL with Docker

When PostgreSQL is running directly on the Windows host, the Docker container should use:

Host=host.docker.internal

instead of:

Host=localhost

This allows the container to connect to the PostgreSQL service running on the Windows host.

Check Running Containers
docker ps
View Container Logs
docker logs techgearapi-container
Stop the Container
docker stop techgearapi-container
Start the Container Again
docker start techgearapi-container
Remove the Container
docker rm -f techgearapi-container

The Docker image can remain available after removing the container.

Docker Verification

The Dockerized API was tested end-to-end.

The following workflow was successfully verified:

Docker Container
       ↓
ASP.NET Core API
       ↓
PostgreSQL Database
       ↓
User Registration
       ↓
User Login
       ↓
JWT Token
       ↓
Authenticated Product Request
       ↓
Products Returned

The protected product endpoint was successfully accessed using a JWT token from the Dockerized application.


Middleware

The project includes custom middleware for:

Global Exception Handling

Provides centralized handling of application exceptions and prevents duplicated exception-handling logic throughout controllers.

Request Logging

Logs incoming HTTP requests and provides visibility into API activity.

DTOs

Data Transfer Objects are used to separate API request/response models from database entities.

Examples include:

User registration DTO
User login DTO
Product creation/update DTO

This helps keep the API contract separate from the underlying database models.

Service Layer

Business logic is separated from controllers through service classes.

This keeps controllers focused on handling HTTP requests and responses while business operations are handled within the service layer.

Postman API Testing

The API was tested using Postman for:

User registration
User login
JWT authentication
Authorization
Protected product endpoints
Product operations


<img width="1920" height="1080" alt="Screenshot (16)" src="https://github.com/user-attachments/assets/f6b2f1d0-5649-4f22-a006-f7d2cb65a139" />
<img width="1920" height="1080" alt="Screenshot (15)" src="https://github.com/user-attachments/assets/bb446dbd-f1d5-48e8-bdd8-c87fdc0b1e68" />
<img width="1920" height="1080" alt="Screenshot (14)" src="https://github.com/user-attachments/assets/28d08c7a-a8a9-49da-b3d4-6b85830f983e" />
<img width="1920" height="1080" alt="Screenshot (13)" src="https://github.com/user-attachments/assets/c3d4a788-ecf9-4588-ba60-d2fc93e8b98a" />
API testing using Postman for Authorization.

Learning Objectives

This project was developed to practice production-oriented backend development concepts, including:

RESTful API design
ASP.NET Core Web API
JWT authentication
Role-based authorization
Secure password hashing
Entity Framework Core
PostgreSQL database integration
Layered architecture
Service layer architecture
Dependency injection
Middleware
DTOs
Database migrations
Docker containerization
API testing with Postman

Future Improvements

Possible future improvements include:

Swagger/OpenAPI documentation
Refresh token authentication
Automated unit and integration tests
Docker Compose for API + PostgreSQL
CI/CD using GitHub Actions
Production deployment
Centralized secret management
API rate limiting
Improved API versioning

Author
Developed as a backend development project to practice ASP.NET Core Web API, PostgreSQL, Entity Framework Core, authentication, authorization, RESTful API design, and Docker containerization.

This project was built to practice production-oriented backend development concepts such as secure authentication, authorization, clean architecture, and RESTful API design.
