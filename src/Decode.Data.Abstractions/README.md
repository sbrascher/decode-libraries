# Decode.Data.Abstractions

Core abstractions for the **Decode.Data** ecosystem, defining contracts for connection and transaction management in .NET.

## 📦 Installation

```bash
dotnet add package Decode.Data.Abstractions
```

## 🛠️ Interfaces

### IDbSession
Defines the contract for managing a database session and its associated transaction.

```csharp
public interface IDbSession : IDisposable, IAsyncDisposable
{
    DbTransaction? Transaction { get; set; }
    Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
```

### IUnitOfWork
Defines the contract for managing atomic transactions.

```csharp
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
```

## 📖 Why Abstractions?

By depending on these interfaces instead of concrete implementations, your domain and application layers remain agnostic to specific database providers or the internal implementation details of connection management.

## 📄 License
MIT License.
