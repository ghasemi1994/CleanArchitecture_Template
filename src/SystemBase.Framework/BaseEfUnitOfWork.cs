
namespace SystemBase.Framework;

public abstract class BaseEfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : BaseDbContext
{

    private readonly TDbContext _dbContext;
    protected BaseEfUnitOfWork(TDbContext dbContext)
    {
       _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.BeginTransactionAsync(cancellationToken); 
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
       await _dbContext.Database.RollbackTransactionAsync(cancellationToken);   
    }
}
