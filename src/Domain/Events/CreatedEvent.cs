using Domain.Common;

namespace Domain.Events;

public class CreatedEvent<T>(T item) : DomainEvent where T : class, IEntity
{
    public T Item { get; } = item;
}
