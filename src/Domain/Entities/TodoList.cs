using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(nameof(TodoList))]
public class TodoList : AuditableEntity, IHasDomainEvent
{
    public string Title { get; set; }

    public Colors Color { get; set; } = Colors.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();

    public List<DomainEvent> DomainEvents { get; set; } = [];
}
