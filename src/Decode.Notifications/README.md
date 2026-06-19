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

// You can use AddDomainNotifications() or the alias AddDecodeNotifications()
builder.Services.AddDomainNotifications();
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

### 3. The `DomainNotification` Model & Context Operations

Under the hood, notifications are stored as `DomainNotification` objects in the context.

#### DomainNotification Class Structure

- **Message (`string`):** The validation or error message.
- **StatusCode (`int`):** The HTTP status code representation (defaults to `400`).
- **Key (`string?`):** Optional parameter representing the field name or key (e.g. `nameof(username)`).

#### Additional Context Methods

You can also add individual notifications directly or batch multiple notifications:

```csharp
// Add an explicit DomainNotification object
_notificationContext.Add(new DomainNotification("Email is invalid", 422, "Email"));

// Add a collection of DomainNotifications at once (useful for batch validations)
var notificationBatch = new List<DomainNotification>
{
    new DomainNotification("Password is too short", 400, "Password"),
    new DomainNotification("Password must contain a number", 400, "Password")
};
_notificationContext.AddRange(notificationBatch);

// Retrieve all raw notifications currently in the context
IReadOnlyCollection<DomainNotification> rawNotifications = _notificationContext.Notifications;
```

### 4. Handling Notifications in Controllers/Middleware

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
