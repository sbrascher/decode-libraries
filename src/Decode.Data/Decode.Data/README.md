# Decode.Data

Lightweight and agnostic implementation of **Unit of Work** and **DbSession** patterns for managing database connections and transactions in .NET.

## 📦 Installation

```bash
dotnet add package Decode.Data
```

## 🛠️ Usage

### 1. Dependency Injection Configuration

In your `Program.cs` or `Startup.cs`:

```csharp
using Decode.Data.Extensions;
using Microsoft.Data.SqlClient; // Or any other provider of your choice

builder.Services.AddDbSessionAndUnitOfWork(sp => 
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 2. Using in Repositories or Services

```csharp
public class UserRepository
{
    private readonly IDbSession _session;

    public UserRepository(IDbSession session)
    {
        _session = session;
    }

    public async Task AddAsync(User user)
    {
        var connection = await _session.CreateConnectionAsync();
        // Use your favorite tool (Dapper, ADO.NET, etc.)
        // connection.Execute("INSERT...", user, _session.Transaction);
    }
}
```

### 3. Managing Transactions with Unit of Work

```csharp
public class UserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _repo;

    public UserService(IUnitOfWork uow, IUserRepository repo)
    {
        _uow = uow;
        _repo = repo;
    }

    public async Task CreateUserAsync(User user)
    {
        await _uow.BeginTransactionAsync();
        try 
        {
            await _repo.AddAsync(user);
            await _uow.CommitAsync();
        }
        catch 
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
```

## 📄 License
MIT License.
