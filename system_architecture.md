# Education Platform - System Architecture

```mermaid
graph TB
    %% User Interface Layer
    subgraph "Presentation Layer"
        UI[Web Browser]
        UI --> MVC[MVC Controllers]
        MVC --> Views[Views/Razor Pages]
        Views --> Bootstrap[Bootstrap 5 UI]
    end

    %% Business Logic Layer
    subgraph "Application Layer"
        Controllers[Controllers]
        ViewModels[ViewModels]
        Services[Business Services]
    end

    %% Data Access Layer
    subgraph "Data Layer"
        EF[Entity Framework Core]
        DbContext[ApplicationDbContext]
        Migrations[EF Migrations]
    end

    %% Database Layer
    subgraph "Database"
        SQL[SQL Server Database]
        Identity[ASP.NET Identity Tables]
        AppData[Application Data Tables]
    end

    %% External Services
    subgraph "External Services"
        FileSystem[File System Storage]
        IdentityService[Identity Service]
    end

    %% Connections
    UI --> MVC
    MVC --> Controllers
    Controllers --> ViewModels
    Controllers --> Services
    Services --> EF
    EF --> DbContext
    DbContext --> SQL
    DbContext --> IdentityService
    Controllers --> FileSystem

    %% Styling
    classDef presentationLayer fill:#e1f5fe
    classDef applicationLayer fill:#f3e5f5
    classDef dataLayer fill:#e8f5e8
    classDef databaseLayer fill:#fff3e0
    classDef externalLayer fill:#fce4ec

    class UI,Views,Bootstrap presentationLayer
    class Controllers,ViewModels,Services applicationLayer
    class EF,DbContext,Migrations dataLayer
    class SQL,Identity,AppData databaseLayer
    class FileSystem,IdentityService externalLayer
```

## Architecture Components

### **1. Presentation Layer**
- **Web Browser**: Client-side interface
- **MVC Controllers**: Handle HTTP requests
- **Views**: Razor pages with Bootstrap 5
- **Static Assets**: CSS, JS, Images, Files

### **2. Application Layer**
- **Controllers**: 17 controllers handling different features
- **ViewModels**: 24 view models for data transfer
- **Business Logic**: Application services and validation

### **3. Data Layer**
- **Entity Framework Core**: ORM for database access
- **ApplicationDbContext**: Main database context
- **Migrations**: Database schema management

### **4. Database Layer**
- **SQL Server**: Primary database
- **Identity Tables**: User authentication/authorization
- **Application Tables**: Business data (Classes, Assignments, etc.)

### **5. External Services**
- **File System**: File storage for assignments/books
- **Identity Service**: User management and authentication

## Technology Stack

```mermaid
graph LR
    subgraph "Frontend"
        HTML[HTML5]
        CSS[Bootstrap 5]
        JS[jQuery]
    end

    subgraph "Backend"
        ASPNET[ASP.NET Core 9.0]
        MVC[MVC Pattern]
        EF[Entity Framework Core]
    end

    subgraph "Database"
        SQL[SQL Server]
        Identity[ASP.NET Identity]
    end

    subgraph "Development"
        NET[.NET 9.0]
        CSharp[C#]
        Razor[Razor Views]
    end

    HTML --> ASPNET
    CSS --> ASPNET
    JS --> ASPNET
    ASPNET --> EF
    EF --> SQL
    ASPNET --> Identity
```

## Data Flow Architecture

```mermaid
sequenceDiagram
    participant U as User
    participant C as Controller
    participant VM as ViewModel
    participant S as Service
    participant EF as Entity Framework
    participant DB as Database

    U->>C: HTTP Request
    C->>VM: Create ViewModel
    C->>S: Call Business Logic
    S->>EF: Query/Update Data
    EF->>DB: SQL Operations
    DB-->>EF: Return Data
    EF-->>S: Entity Objects
    S-->>C: Business Results
    C->>VM: Populate ViewModel
    C->>U: Return View/Response
```

## Security Architecture

```mermaid
graph TB
    subgraph "Authentication"
        Login[Login Form]
        Identity[ASP.NET Identity]
        Roles[Role Management]
    end

    subgraph "Authorization"
        Authorize[Authorize Attributes]
        Policies[Role-based Policies]
        Claims[User Claims]
    end

    subgraph "Data Security"
        Validation[Input Validation]
        CSRF[CSRF Protection]
        HTTPS[HTTPS Enforcement]
    end

    Login --> Identity
    Identity --> Roles
    Roles --> Authorize
    Authorize --> Policies
    Policies --> Claims
    Claims --> Validation
    Validation --> CSRF
    CSRF --> HTTPS
```

## File Storage Architecture

```mermaid
graph LR
    subgraph "File Upload"
        Upload[File Upload]
        Validation[File Validation]
        Storage[File Storage]
    end

    subgraph "File Types"
        PDF[PDF Files]
        Images[Image Files]
        Documents[Document Files]
    end

    subgraph "Storage Locations"
        Assignments[Assignments Folder]
        Books[Books Folder]
        Submissions[Submissions Folder]
    end

    Upload --> Validation
    Validation --> Storage
    Storage --> PDF
    Storage --> Images
    Storage --> Documents
    PDF --> Assignments
    Images --> Books
    Documents --> Submissions
```

## Key Features by Layer

### **Presentation Layer Features:**
- Responsive Bootstrap 5 UI
- Role-based navigation
- File upload interfaces
- Real-time notifications
- Exam taking interface

### **Application Layer Features:**
- Role-based authorization
- Business logic validation
- File processing
- Notification system
- Grade calculation

### **Data Layer Features:**
- Entity relationships
- JSON data storage
- Composite keys
- Cascade deletes
- Migration management

### **Database Layer Features:**
- User authentication
- Academic data management
- File metadata storage
- Audit trails
- Performance optimization 