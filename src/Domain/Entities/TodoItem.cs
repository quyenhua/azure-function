using Domain.Common;
using Domain.Enums;
using Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table(nameof(TodoItem))]
public class TodoItem : AuditableEntity, IHasDomainEvent
{
    public TodoList List { get; set; }

    public int ListId { get; set; }

    public string Title { get; set; }

    public string Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                DomainEvents.Add(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public List<DomainEvent> DomainEvents { get; set; } = [];
}
