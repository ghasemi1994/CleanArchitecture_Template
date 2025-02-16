
namespace SystemBase.Framework;

public interface IRepository<TAggregate> where TAggregate : IAggregateRoot
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void SaveChanges();
}
