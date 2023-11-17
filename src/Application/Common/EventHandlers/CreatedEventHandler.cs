using MediatR;
using Microsoft.Extensions.Logging;

using Application.Common.Models;
using Domain.Events;
using Domain.Common;

namespace Application.Common.EventHandlers;

public class CreatedEventHandler<T>(ILogger<CreatedEventHandler<T>> logger)
    : INotificationHandler<DomainEventNotification<CreatedEvent<T>>> where T : class , IEntity
{
    private readonly ILogger<CreatedEventHandler<T>> _logger = logger;

    public Task Handle(DomainEventNotification<CreatedEvent<T>> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Azure Function Domain Created Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
