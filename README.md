# Generic CRUD Controller for Master Entities

A generic CRUD implementation in .NET 8 using Entity Framework Core with SQL Server, following a layered architecture pattern (Controller → Service → Repository).

## Features

- **Generic CRUD Operations**: Reusable components for common CRUD operations
- **Soft Delete**: Implemented using `IsActive` flag
- **Pagination & Filtering**: Built-in support for paginated results and filtering
- **DTOs & AutoMapper**: Data Transfer Objects with automatic mapping
- **Dependency Injection**: Proper DI setup for all layers
- **Error Handling & Logging**: Comprehensive error handling and logging
- **Swagger Integration**: API documentation with Swagger

## Architecture

```
Controller Layer → Service Layer → Repository Layer → Database
```

### Layers

1. **Controller**: `GenericMasterController<T, TDto>` - Handles HTTP requests
2. **Service**: `IGenericService<T, TDto>` & `GenericService<T, TDto>` - Business logic
3. **Repository**: `IGenericRepository<T>` & `GenericRepository<T>` - Data access
4. **Data**: `AppDbContext` - Entity Framework context

## Project Structure

```
GenericController/
├── Controllers/
│   └── GenericMasterController.cs
├── Services/
│   ├── IGenericService.cs
│   └── GenericService.cs
├── Repositories/
│   ├── IGenericRepository.cs
│   └── GenericRepository.cs
├── Models/
│   ├── BaseEntity.cs
│   ├── MstMarketingNote.cs
│   ├── MstLegalChecklist.cs
│   └── MstCommercialChecklist.cs
├── DTOs/
│   ├── MstMarketingNoteDto.cs
│   ├── MstLegalChecklistDto.cs
│   └── MstCommercialChecklistDto.cs
├── Data/
│   └── AppDbContext.cs
├── MappingProfiles/
│   └── MappingProfile.cs
├── Program.cs
├── appsettings.json
├── .gitignore
└── README.md
```

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd GenericController
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**
   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_username;Password=your_password;TrustServerCertificate=True;"
     }
   }
   ```

4. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access Swagger UI**
   Navigate to `https://localhost:5001/swagger` (or your configured port)

## API Endpoints

### Generic CRUD Operations

All master entities support the following endpoints:

- `GET /api/{entity}` - Get all items
- `GET /api/{entity}/{id}` - Get item by ID
- `POST /api/{entity}` - Create new item
- `PUT /api/{entity}/{id}` - Update item
- `DELETE /api/{entity}/{id}` - Hard delete item
- `DELETE /api/{entity}/{id}/soft` - Soft delete item
- `GET /api/{entity}/paged?pageNumber=1&pageSize=10&filter=optional` - Get paginated results

### Example Usage

#### Marketing Notes
- `GET /api/MstMarketingNote`
- `POST /api/MstMarketingNote`

#### Legal Checklist
- `GET /api/MstLegalChecklist`
- `POST /api/MstLegalChecklist`

#### Commercial Checklist
- `GET /api/MstCommercialChecklist`
- `POST /api/MstCommercialChecklist`

## Adding New Master Entities

1. **Create Entity Model**
   ```csharp
   public class NewMasterEntity : BaseEntity
   {
       [Key]
       public int Id { get; set; }
       // Add your properties
   }
   ```

2. **Create DTO**
   ```csharp
   public class NewMasterEntityDto
   {
       public int Id { get; set; }
       // Add your properties
       public bool IsActive { get; set; } = true;
   }
   ```

3. **Update DbContext**
   ```csharp
   public DbSet<NewMasterEntity> NewMasterEntities { get; set; }
   ```

4. **Update Mapping Profile**
   ```csharp
   CreateMap<NewMasterEntity, NewMasterEntityDto>().ReverseMap();
   ```

5. **Register in Program.cs**
   ```csharp
   builder.Services.AddScoped<IGenericService<NewMasterEntity, NewMasterEntityDto>, GenericService<NewMasterEntity, NewMasterEntityDto>>();
   ```

6. **Create Specific Controller (Optional)**
   ```csharp
   [Route("api/[controller]")]
   public class NewMasterController : GenericMasterController<NewMasterEntity, NewMasterEntityDto>
   {
       public NewMasterController(IGenericService<NewMasterEntity, NewMasterEntityDto> service, ILogger<NewMasterController> logger)
           : base(service, logger)
       {
       }
   }
   ```

## Database Schema

The implementation supports three master entities based on the provided SQL schema:

### MstMarketingNote
- NoteCode (PK)
- Description, IsChildOf, SortSeq, IsDeleted, ControlType, Value, IsFileUpload, FileLibrary, IsMandatory, ValidationType

### MstLegalChecklist
- LegalChkId (PK, Identity)
- SrNo, ScreenPoint, Recommendation, ApprovalForDeviation, SortSeq, IsDeleted, ControlType, Value, IsFileUpload, IsMandetory, ValidationType, Termsnconditions

### MstCommercialChecklist
- ChkId (PK)
- Particulars, SortSeq, IsDeleted, ControlType, Value, IsFileUpload, IsMandetory, ValidationType, CommercialPolicy, ForCosting

## Technologies Used

- .NET 8
- Entity Framework Core 8.0
- SQL Server
- AutoMapper 13.0.1
- Swagger/OpenAPI
- Dependency Injection
- Logging

## Best Practices Implemented

- **Separation of Concerns**: Clear layer separation
- **Dependency Injection**: Proper DI throughout the application
- **Error Handling**: Try-catch blocks with logging
- **Soft Delete**: Using IsActive flag instead of hard deletes
- **DTO Pattern**: Separation of data models and transfer objects
- **Generic Programming**: Reusable components
- **Async/Await**: Asynchronous operations
- **Logging**: Comprehensive logging for debugging

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.