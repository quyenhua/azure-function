using MediatR;

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Common;

namespace Application.Features.Generics.Commands;

public class DeleteCommand<T> : IRequest<int> where T : class, IEntity
{
    public int Id { get; set; }
}

public class DeleteCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<DeleteCommand<T>, int> where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<int> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id) ?? throw new NotFoundException(typeof(T).Name, request.Id);

        await _repository.DeleteAsync(entity);
        return await _repository.SaveChangesAsync(cancellationToken);
    }
}
