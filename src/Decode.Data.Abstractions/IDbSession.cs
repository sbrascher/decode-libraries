using System.Data.Common;

namespace Decode.Data.Abstractions;

public interface IDbSession : IDisposable, IAsyncDisposable
{
    DbTransaction? Transaction { get; set; }
    Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}