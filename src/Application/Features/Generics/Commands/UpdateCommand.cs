using MediatR;

using Application.Common.Interfaces;
using Domain.Common;

namespace Application.Features.Generics.Commands;

public class UpdateCommand<T> : IRequest<int> where T : class, IEntity
{
    public required T Entity { get; set; }
}

public class UpdateCommandHandler<T>(IGenericRepository<T> repository) : IRequestHandler<UpdateCommand<T>, int> where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<int> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(request.Entity);
        return await _repository.SaveChangesAsync(cancellationToken);
    }
}
