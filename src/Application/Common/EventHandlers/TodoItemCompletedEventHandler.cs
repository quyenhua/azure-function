using MediatR;
using Microsoft.Extensions.Logging;

using Application.Common.Models;
using Domain.Events;

namespace Application.Common.EventHandlers;

public class TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    : INotificationHandler<DomainEventNotification<TodoItemCompletedEvent>>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger = logger;

    public Task Handle(DomainEventNotification<TodoItemCompletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Azure Function Domain Completed Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
