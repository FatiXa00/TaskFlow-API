# 🚀 TaskFlow API

A **production-ready Real-Time Task Management REST API** built with **ASP.NET Core 8**, showcasing **Clean Architecture**, **JWT Authentication**, **SignalR Real-Time Updates**, and comprehensive **Unit Testing**.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core%208-512BD4?style=flat)
![SignalR](https://img.shields.io/badge/SignalR-Real--Time-FF6600?style=flat)
![Tests](https://img.shields.io/badge/Tests-14%20Passed-success?style=flat)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)

---

## ✨ Key Features

### 🏗️ **Architecture & Design Patterns**
- ✅ **Clean Architecture** with 4 distinct layers (Domain, Application, Infrastructure, API)
- ✅ **Repository Pattern** for data access abstraction
- ✅ **Dependency Injection** throughout the application
- ✅ **DTOs** (Data Transfer Objects) for secure data handling
- ✅ **SOLID Principles** implementation

### 🔐 **Security**
- ✅ **JWT Bearer Authentication** with secure token generation
- ✅ **BCrypt Password Hashing** for secure storage
- ✅ **Authorization** with role-based access control
- ✅ User can only modify/delete their own tasks

### 🔥 **Real-Time Features**
- ✅ **SignalR WebSocket Integration** for instant updates
- ✅ Live task creation notifications
- ✅ Real-time task status updates
- ✅ Instant task deletion broadcasts
- ✅ Multi-client synchronization

### 📊 **API Features**
- ✅ Complete **CRUD Operations** for Tasks and Users
- ✅ Task filtering by status (Todo, InProgress, Done)
- ✅ Task prioritization (Low, Medium, High, Urgent)
- ✅ User authentication (Register/Login)
- ✅ RESTful API design with proper HTTP status codes
- ✅ **Swagger/OpenAPI** documentation

### 🧪 **Testing & Quality**
- ✅ **14 Unit Tests** with xUnit
- ✅ **Moq** for dependency mocking
- ✅ **FluentAssertions** for readable test assertions
- ✅ **InMemory Database** for isolated testing
- ✅ **100% test coverage** on repositories

### 🗄️ **Database**
- ✅ **Entity Framework Core** with Code-First approach
- ✅ **SQLite** for lightweight persistence
- ✅ **Migrations** for version control
- ✅ **One-to-Many relationships** properly configured
- ✅ **Indexes** for performance optimization

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
TaskFlow/
│
├── 📦 TaskFlow.Domain/              # Core Business Layer
│   └── Entities/
│       ├── User.cs                  # User entity
│       ├── TaskItem.cs              # Task entity
│       ├── TaskStatus.cs            # Status enum (Todo, InProgress, Done)
│       └── TaskPriority.cs          # Priority enum (Low, Medium, High, Urgent)
│
├── 📦 TaskFlow.Application/         # Business Logic Layer
│   ├── DTOs/
│   │   ├── TaskDto.cs               # Task data transfer object
│   │   ├── CreateTaskDto.cs         # Create task request
│   │   ├── UpdateTaskDto.cs         # Update task request
│   │   ├── UserDto.cs               # User data transfer object
│   │   ├── RegisterDto.cs           # Registration request
│   │   ├── LoginDto.cs              # Login request
│   │   └── AuthResponseDto.cs       # Authentication response with JWT
│   └── Interfaces/
│       ├── IRepository<T>.cs        # Generic repository interface
│       ├── ITaskRepository.cs       # Task-specific operations
│       ├── IUserRepository.cs       # User-specific operations
│       └── IAuthService.cs          # Authentication service interface
│
├── 📦 TaskFlow.Infrastructure/      # Data Access Layer
│   ├── Data/
│   │   └── ApplicationDbContext.cs  # EF Core DbContext
│   ├── Repositories/
│   │   ├── Repository<T>.cs         # Generic repository implementation
│   │   ├── TaskRepository.cs        # Task repository
│   │   └── UserRepository.cs        # User repository
│   └── Services/
│       └── AuthService.cs           # JWT authentication service
│
├── 📦 TaskFlow (API)/               # Presentation Layer
│   ├── Controllers/
│   │   ├── AuthController.cs        # Authentication endpoints
│   │   ├── TasksController.cs       # Task CRUD + SignalR
│   │   └── UsersController.cs       # User management
│   ├── Hubs/
│   │   └── TaskHub.cs               # SignalR hub for real-time updates
│   ├── wwwroot/
│   │   └── realtime-demo.html       # Real-time demo page
│   ├── Program.cs                   # App configuration & DI
│   └── appsettings.json             # Configuration (DB, JWT)
│
└── 📦 TaskFlow.Tests/               # Test Layer
    ├── RepositoryTests/
    │   ├── TaskRepositoryTests.cs   # 8 unit tests
    │   └── UserRepositoryTests.cs   # 6 unit tests
    └── ServiceTests/                # (Extensible for future tests)
```

### 🔄 Dependency Flow (Clean Architecture)
```
API → Infrastructure → Application → Domain
 ↓         ↓              ↓            ↓
Controllers  Repos    Interfaces   Entities
             Services    DTOs
```

**Benefits:**
- **Testability**: Each layer can be tested independently
- **Maintainability**: Changes in one layer don't cascade
- **Flexibility**: Easy to swap implementations (e.g., switch database)
- **Scalability**: Clear structure for adding features

---

## 🛠️ Technologies & Tools

| Category | Technology |
|----------|-----------|
| **Framework** | ASP.NET Core 8.0 |
| **Language** | C# 12 |
| **ORM** | Entity Framework Core 8.0 |
| **Database** | SQLite |
| **Authentication** | JWT Bearer Tokens |
| **Password Hashing** | BCrypt.Net |
| **Real-Time** | SignalR (WebSockets) |
| **Testing** | xUnit, Moq, FluentAssertions |
| **API Documentation** | Swagger/OpenAPI |
| **Architecture** | Clean Architecture |

---

## 📦 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0.121 or higher)
- Visual Studio 2022+ / VS Code / Rider
- Git

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/FatiXa00/TaskFlow-API.git
cd TaskFlow-API
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Apply database migrations**
```bash
cd TaskFlow.Infrastructure
dotnet ef database update --startup-project ../TaskFlow
cd ..
```

Or using Package Manager Console in Visual Studio:
```powershell
Update-Database -StartupProject TaskFlow -Project TaskFlow.Infrastructure
```

4. **Run the application**
```bash
cd TaskFlow
dotnet run
```

5. **Access the API**
- Swagger UI: `https://localhost:7xxx/swagger`
- Real-Time Demo: `https://localhost:7xxx/realtime-demo.html`

---

## 🧪 Running Tests

### Via Command Line
```bash
dotnet test
```

### Via Visual Studio
1. Open **Test Explorer** (Test → Test Explorer)
2. Click **Run All Tests**
3. ✅ All 14 tests should pass!

**Test Coverage:**
- 8 TaskRepository tests (CRUD, filtering, user-specific queries)
- 6 UserRepository tests (CRUD, email/username lookup)

---

## 🔌 API Endpoints

### 🔐 Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `POST` | `/api/auth/register` | Create new user account | ❌ |
| `POST` | `/api/auth/login` | Authenticate and get JWT token | ❌ |

### 👤 Users

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/users` | Get all users | ✅ |
| `GET` | `/api/users/{id}` | Get user by ID | ✅ |
| `GET` | `/api/users/email/{email}` | Get user by email | ✅ |
| `DELETE` | `/api/users/{id}` | Delete user | ✅ |

### 📋 Tasks

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/tasks` | Get all tasks | ✅ |
| `GET` | `/api/tasks/{id}` | Get task by ID | ✅ |
| `GET` | `/api/tasks/my-tasks` | Get authenticated user's tasks | ✅ |
| `GET` | `/api/tasks/user/{userId}` | Get tasks by user ID | ✅ |
| `GET` | `/api/tasks/status/{status}` | Get tasks by status (0=Todo, 1=InProgress, 2=Done) | ✅ |
| `POST` | `/api/tasks` | Create new task | ✅ |
| `PUT` | `/api/tasks/{id}` | Update task (own tasks only) | ✅ |
| `DELETE` | `/api/tasks/{id}` | Delete task (own tasks only) | ✅ |

### 🔥 SignalR Hub

**Endpoint:** `/hubs/task`

**Events:**
- `TaskCreated` - Broadcasted when a task is created
- `TaskUpdated` - Broadcasted when a task is updated
- `TaskDeleted` - Broadcasted when a task is deleted

---

## 📝 Usage Examples

### 1. Register a New User

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "test",
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "User registered successfully"
}
```

---

### 2. Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

**💡 Copy the token and use it in subsequent requests!**

---

### 3. Create a Task (with JWT)

```http
POST /api/tasks
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API docs",
  "priority": 2,
  "dueDate": "2025-10-30T12:00:00Z"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Complete project documentation",
  "description": "Write comprehensive README and API docs",
  "status": "Todo",
  "priority": "High",
  "createdAt": "2025-10-21T14:30:00Z",
  "updatedAt": null,
  "dueDate": "2025-10-30T12:00:00Z",
  "createdByUserId": "...",
  "createdByUsername": "test"
}
```

---

### 4. Update Task Status

```http
PUT /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "status": 1,
  "priority": 3
}
```

**Status Values:**
- `0` = Todo
- `1` = InProgress
- `2` = Done

**Priority Values:**
- `0` = Low
- `1` = Medium
- `2` = High
- `3` = Urgent

---

### 5. Get My Tasks

```http
GET /api/tasks/my-tasks
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:** Array of tasks created by the authenticated user

---

## 🔥 Real-Time Demo

### Testing SignalR in Action

1. **Start the application** and navigate to `/realtime-demo.html`
2. **Open TWO browser tabs** side-by-side with the same page
3. **Login in both tabs** with the same credentials
4. **Create a task in Tab 1**
5. **Watch Tab 2** → The task appears **instantly** without refresh! ⚡

**Or test with Swagger:**
1. Keep the demo page open
2. Create/Update/Delete tasks via Swagger
3. Watch the demo page update in real-time!

**JavaScript Client Example:**
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/task", {
        accessTokenFactory: () => yourJwtToken
    })
    .build();

// Listen for task creation
connection.on("TaskCreated", (task) => {
    console.log("New task created:", task);
    // Update UI in real-time
});

await connection.start();
```

---

## 🗄️ Database Schema

### Users Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | GUID | PRIMARY KEY |
| Username | TEXT | UNIQUE, NOT NULL |
| Email | TEXT | UNIQUE, NOT NULL |
| PasswordHash | TEXT | NOT NULL |
| CreatedAt | DATETIME | NOT NULL |

### Tasks Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | GUID | PRIMARY KEY |
| Title | TEXT | NOT NULL |
| Description | TEXT | NULL |
| Status | INTEGER | NOT NULL (0=Todo, 1=InProgress, 2=Done) |
| Priority | INTEGER | NOT NULL (0=Low, 1=Medium, 2=High, 3=Urgent) |
| CreatedAt | DATETIME | NOT NULL |
| UpdatedAt | DATETIME | NULL |
| DueDate | DATETIME | NULL |
| CreatedByUserId | GUID | FOREIGN KEY → Users(Id), ON DELETE CASCADE |

**Indexes:**
- `Users.Email` (UNIQUE)
- `Users.Username` (UNIQUE)
- `Tasks.Status`
- `Tasks.CreatedByUserId`

---

## 🎯 Skills Demonstrated

### Backend Development
- ✅ **ASP.NET Core** Web API development
- ✅ **Clean Architecture** implementation
- ✅ **Repository Pattern** for data abstraction
- ✅ **Dependency Injection** (DI)
- ✅ **LINQ** for data querying
- ✅ **Async/Await** for non-blocking operations

### Security
- ✅ **JWT Authentication** with Bearer tokens
- ✅ **BCrypt** password hashing
- ✅ **Authorization** filters
- ✅ Secure data transfer with DTOs

### Database
- ✅ **Entity Framework Core** ORM
- ✅ **Code-First** migrations
- ✅ **One-to-Many relationships**
- ✅ Database **indexing** for performance

### Real-Time Features
- ✅ **SignalR** WebSocket integration
- ✅ Real-time event broadcasting
- ✅ Connection management

### Testing
- ✅ **Unit Testing** with xUnit
- ✅ **Mocking** dependencies with Moq
- ✅ **Fluent Assertions** for readable tests
- ✅ **InMemory Database** for test isolation

### API Design
- ✅ **RESTful** API principles
- ✅ Proper **HTTP status codes**
- ✅ **Swagger/OpenAPI** documentation
- ✅ **CORS** configuration

---

## 🚀 Future Enhancements

### Planned Features
- [ ] Role-based authorization (Admin, User roles)
- [ ] Refresh tokens for extended sessions
- [ ] Email verification on registration
- [ ] Password reset functionality
- [ ] Task comments and attachments
- [ ] Task assignments to multiple users
- [ ] Notifications (email, push)
- [ ] Pagination and advanced filtering
- [ ] Rate limiting for API protection
- [ ] Docker containerization
- [ ] CI/CD pipeline with GitHub Actions
- [ ] Logging with Serilog
- [ ] Caching with Redis
- [ ] Integration tests
- [ ] Performance benchmarking

---

## 📚 Learning Resources

This project demonstrates concepts from:
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microsoft's ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [SignalR Documentation](https://docs.microsoft.com/en-us/aspnet/core/signalr/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Fatima Zahraa Sahraoui**
- GitHub: [@FatiXa00](https://github.com/FatiXa00)

---

## 🙏 Acknowledgments

- Built as a portfolio project to demonstrate full-stack .NET proficiency
- Inspired by modern task management applications (Trello, Asana, Todoist)
- Special thanks to the .NET community for excellent documentation and resources

---

## 📊 Project Stats

![GitHub repo size](https://img.shields.io/github/repo-size/FatiXa00/TaskFlow-API)
![GitHub last commit](https://img.shields.io/github/last-commit/FatiXa00/TaskFlow-API)
![GitHub issues](https://img.shields.io/github/issues/FatiXa00/TaskFlow-API)

---

⭐ **If you found this project helpful or impressive, please give it a star!** ⭐

**Built with 💜 using .NET 8 and Clean Architecture principles**
