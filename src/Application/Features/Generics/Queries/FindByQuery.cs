using System.Linq.Expressions;

using MediatR;

using Application.Common.Interfaces;
using Domain.Common;

namespace Application.Features.Generics.Queries;

public class FindByQuery<T> : IRequest<IQueryable<T>> where T : class, IEntity
{
    public Expression<Func<T, bool>> Predicate { get; set; }
}

public class FindByQueryHandler<T>(IGenericRepository<T> repository) 
    : IRequestHandler<FindByQuery<T>, IQueryable<T>> where T : class, IEntity
{
    private readonly IGenericRepository<T> _repository = repository;

    public async Task<IQueryable<T>> Handle(FindByQuery<T> request, CancellationToken cancellationToken)
    {
        return _repository.Entities.Where(request.Predicate);
    }
}
