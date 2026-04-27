# Decode.Notifications.FluentValidation

FluentValidation extensions for **Decode.Notifications** library.

## 📦 Installation

```bash
dotnet add package Decode.Notifications.FluentValidation
```

## 🛠️ Usage

### Integrating FluentValidation with Notification Context

```csharp
using Decode.Notifications.FluentValidation;
using FluentValidation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UserService
{
    private readonly IDomainNotificationContext _context;
    private readonly IValidator<User> _validator;

    public UserService(IDomainNotificationContext context, IValidator<User> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task CreateUser(User user)
    {
        var result = await _validator.ValidateAsync(user);

        // This will map all FluentValidation errors to the Notification Context
        result.AddToContext(_context);

        if (_context.HasNotifications)
            return;

        // Save user...
    }
}
```

## 📄 License
MIT License.
