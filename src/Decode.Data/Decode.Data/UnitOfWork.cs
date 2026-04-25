using Decode.Data.Abstractions;
using System.Data.Common;

namespace Decode.Data;
public sealed class UnitOfWork : IUnitOfWork
{

    private readonly IDbSession _session;

    public UnitOfWork(IDbSession session)
    {
        _session = session;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_session.Transaction == null)
        {
            DbConnection? connection = await _session.CreateConnectionAsync(cancellationToken);
            _session.Transaction = await connection.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_session.Transaction is not null)
            {
                await _session.Transaction.CommitAsync(cancellationToken);
            }
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_session.Transaction is not null)
            {
                await _session.Transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            await DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_session.Transaction is not null)
        {
            await _session.Transaction.DisposeAsync();
            _session.Transaction = null;
        }
    }

    public void Dispose()
    {
        _session.Transaction?.Dispose();
        _session.Transaction = null;
    }
}