# Decode.AspNetCore

ASP.NET Core extensions for the Decode ecosystem, providing standardized infrastructure for APIs, including global exception handling and base controllers integrated with Domain Notifications.

## 🚀 Features

- **GlobalExceptionMiddleware**: A robust safety net that captures unhandled exceptions, logs them with a unique `errorId`, and returns a standardized JSON response.
- **ApiControllerBase**: A base controller that simplifies response handling by automatically checking for domain notifications and returning the appropriate HTTP status code.
- **ApiResponse**: A unified response model for all API endpoints: `{ success, data, errors, errorId }`.

## 📦 Installation

Add the project reference to your API:

```bash
dotnet add reference ../Decode.AspNetCore/Decode.AspNetCore.csproj
```

## 🛠️ Setup

### 1. Register Services

In your `Program.cs`, add the Decode infrastructure:

```csharp
builder.Services.AddDecodeAspNetCore();
```

### 2. Configure Middleware

Add the global exception handler to your request pipeline:

```csharp
var app = builder.Build();

// Should be one of the first middlewares in the pipeline
app.UseGlobalExceptionMiddleware();

app.MapControllers();
app.Run();
```

## 📖 Usage

### Using the Base Controller

Inherit from `ApiControllerBase` to get automatic notification handling:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ApiControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _service.Create(product);

        // If _service added notifications to the context, 
        // Response() will return 400 Bad Request (or the specific status code set).
        // Otherwise, it returns 200 OK.
        return Response(product);
    }
}
```

### Standard Response Format

The library ensures your API always speaks the same language:

**Success (200 OK):**
```json
{
  "success": true,
  "data": { "id": 1, "name": "Product A" },
  "errors": null,
  "errorId": null
}
```

**Error (e.g., 400 Bad Request via Notifications):**
```json
{
  "success": false,
  "data": null,
  "errors": ["Product name is required"],
  "errorId": null
}
```

**Unexpected Error (500 Internal Server Error):**
```json
{
  "success": false,
  "data": null,
  "errors": ["An internal server error occurred. Please try again later."],
  "errorId": "0HMA1B2C3D4E5:00000001"
}
```

## 📄 License
This project is licensed under the MIT License.
