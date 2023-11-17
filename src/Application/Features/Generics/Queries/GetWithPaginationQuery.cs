using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Domain.Common;
using Domain.Enums;

namespace Application.Features.Generics.Queries;

public class GetWithPaginationQuery<T, TMapper> : IRequest<PaginatedList<TMapper>> where T : class, IEntity where TMapper : class
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Expression<Func<T, bool>> Predicate { get; set; }
    public Expression<Func<T, string>>? OrderBy { get; set; }
    public OrderDirection OrderDirection { get; set; } = OrderDirection.ASC;
}

public class GetWithPaginationQueryHandler<T, TMapper>(IGenericRepository<T> repository, IMapper mapper) 
    : IRequestHandler<GetWithPaginationQuery<T, TMapper>, PaginatedList<TMapper>> where T : class, IEntity where TMapper : class
{
    private readonly IGenericRepository<T> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<TMapper>> Handle(GetWithPaginationQuery<T, TMapper> request, CancellationToken cancellationToken)
    {
        var query = _repository.Entities
            .Where(request.Predicate)
            .OrderBy(x => x.Id);

        if (request.OrderBy != null)
            query = request.OrderDirection == OrderDirection.DESC ? query.OrderByDescending(request.OrderBy) : query.OrderBy(request.OrderBy);

        return await query
            .ProjectTo<TMapper>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
