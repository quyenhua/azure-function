using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class TodoItemCompletedEvent(Todo item) : DomainEvent
{
    public Todo Item { get; } = item;
}
