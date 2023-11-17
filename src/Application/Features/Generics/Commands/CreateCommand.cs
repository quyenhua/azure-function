using MediatR;

using Application.Common.Interfaces;
using Domain.Events;
using Domain.Common;

namespace Application.Features.Generics.Commands;

public class CreateCommand<T> : IRequest<int> where T : class, IEntity
{
    public required T Entity { get; set; }
}

public class CreateCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<CreateCommand<T>, int> where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<int> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
    {
        var entity = request.Entity;
        entity.DomainEvents.Add(new CreatedEvent<T>(entity));

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
