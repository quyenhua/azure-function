using Domain.Common;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(nameof(ListTodo))]
public class ListTodo : AuditableEntity, IHasDomainEvent
{
    public string Title { get; set; }

    public string Color { get; set; } = Colors.White;

    public IList<Todo> Items { get; private set; } = new List<Todo>();

    public List<DomainEvent> DomainEvents { get; set; } = [];
}
