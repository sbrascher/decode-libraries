# Decode.Notifications

A simple and lightweight library for domain notifications using the notification pattern in .NET.

## 📦 Installation

```bash
dotnet add package Decode.Notifications
```

## 🛠️ Usage

### 1. Dependency Injection Configuration

In your `Program.cs` or `Startup.cs`:

```csharp
using Decode.Notifications.Extensions;

builder.Services.AddDecodeNotifications();
```

### 2. Using in Services or Domain Logic

```csharp
public class UserService
{
    private readonly IDomainNotificationContext _notificationContext;

    public UserService(IDomainNotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public void RegisterUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            _notificationContext.Add("Username is required.", HttpStatusCode.BadRequest, nameof(username));
            return;
        }

        // Logic...
    }
}
```

### 3. Handling Notifications in Controllers/Middleware

```csharp
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IDomainNotificationContext _notificationContext;

    public UsersController(IDomainNotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    [HttpPost]
    public IActionResult Create(UserDto user)
    {
        // Call service...
        
        if (_notificationContext.HasNotifications)
        {
            return StatusCode(_notificationContext.GetStatusCode(), _notificationContext.GetMessages());
        }

        return Ok();
    }
}
```

## 📄 License
MIT License.
