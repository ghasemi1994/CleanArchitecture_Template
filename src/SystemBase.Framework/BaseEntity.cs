
using MediatR;

namespace SystemBase.Framework;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public int? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }


    private List<INotification> _domainEvents = new();
    public IReadOnlyList<INotification> DomainEvents => _domainEvents;
    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }
    public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
    public void ClearAllDomainEvent() => _domainEvents.Clear();

}

