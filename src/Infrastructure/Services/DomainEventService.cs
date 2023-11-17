using MediatR;
using Microsoft.Extensions.Logging;

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common;

namespace Infrastructure.Services;

public class DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator) : IDomainEventService
{
    private readonly ILogger<DomainEventService> _logger = logger;
    private readonly IPublisher _mediator = mediator;

    public async Task Publish(DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
    }
}