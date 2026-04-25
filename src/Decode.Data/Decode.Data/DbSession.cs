using Decode.Data.Abstractions;
using System.Data.Common;

namespace Decode.Data;

public delegate DbConnection DbConnectionFactory();

public sealed class DbSession : IDbSession
{
    private readonly DbConnectionFactory _connectionFactory;
    private DbConnection? _connection;
    public DbTransaction? Transaction { get; set; }

    public DbSession(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        _connection ??= _connectionFactory();

        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }

        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (Transaction is not null)
        {
            await Transaction.DisposeAsync();
            Transaction = null;
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        _connection?.Dispose();
    }
}