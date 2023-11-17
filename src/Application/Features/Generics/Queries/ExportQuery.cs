using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Common.Interfaces;
using Application.Models.Responses;
using Domain.Common;

namespace Application.Features.Generics.Queries;

public class ExportQuery<T, TMapper> : IRequest<ExportData> where T : class, IEntity where TMapper : class
{
    public string FileName { get; set; } = "ExportFile";


    public Expression<Func<T, bool>> Predicate { get; set; }
}

public class ExportQueryHandler<T, TMapper>(IGenericRepository<T> repository, IMapper mapper, ICsvFileBuilder fileBuilder) : IRequestHandler<ExportQuery<T, TMapper>, ExportData> where T : class, IEntity where TMapper : class
{
    private readonly IGenericRepository<T> _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICsvFileBuilder _fileBuilder = fileBuilder;

    public async Task<ExportData> Handle(ExportQuery<T, TMapper> request, CancellationToken cancellationToken)
    {
        var vm = new ExportData();

        var records = await _repository.Entities.Where(request.Predicate)
                .ProjectTo<T>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        vm.Content = _fileBuilder.BuildTodoItemsFile<T, TMapper>(records);
        vm.ContentType = "text/csv";
        vm.FileName = $"{request.FileName}.csv";

        return await Task.FromResult(vm);
    }
}
