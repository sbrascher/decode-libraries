# Decode.Data

Implementação leve de **Unit of Work** e **DbSession** para gerenciar conexões e transações no .NET.

## 📦 Instalação

```bash
dotnet add package Decode.Data
```

## 🛠️ Como usar

### 1. Configuração

No seu `Program.cs`:

```csharp
using Decode.Data.Extensions;
using Microsoft.Data.SqlClient;

builder.Services.AddDbSessionAndUnitOfWork(sp => 
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 2. Usando em Repositórios

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
        // Use Dapper ou ADO.NET aqui
    }
}
```

### 3. Transações com Unit of Work

```csharp
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
```

## 📄 Licença
MIT.
