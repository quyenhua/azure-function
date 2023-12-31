﻿using System.Reflection;

using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions options,
    IOptions<OperationalStoreOptions> operationalStoreOptions,
    ICurrentUserService currentUserService,
    IDomainEventService domainEventService,
    IDateTime dateTime) : ApiAuthorizationDbContext<ApplicationUser>(options, operationalStoreOptions), IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IDateTime _dateTime = dateTime;
    private readonly IDomainEventService _domainEventService = domainEventService;

    public DbSet<Todo> TodoItems { get; set; }

    public DbSet<ListTodo> TodoLists { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = _dateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.UtcNow;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents()
    {
        while (true)
        {
            var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .FirstOrDefault();
            if (domainEventEntity == null) break;

            domainEventEntity.IsPublished = true;
            await _domainEventService.Publish(domainEventEntity);
        }
    }
}
