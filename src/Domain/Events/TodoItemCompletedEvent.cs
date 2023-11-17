using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class TodoItemCompletedEvent(TodoItem item) : DomainEvent
{
    public TodoItem Item { get; } = item;
}
