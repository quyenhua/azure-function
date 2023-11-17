using System.Reflection;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

using Application.Common.Behaviors;
using Application.Features.Generics.Commands;
using Application.Features.Generics.Queries;
using Application.Common.Models;
using Application.Models.Responses;
using Domain.Entities;
using Domain.Common;
using Application.Common.Interfaces;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddMediator();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        return services;
    }

    private static void AddMediator(this IServiceCollection services)
    {
        services
            .AddScopedMediatR<TodoList>()
            .AddScopedMediatR<TodoItem>()
            .AddScoped<IRequestHandler<GetWithPaginationQuery<TodoItem, TodoItemDto>, PaginatedList<TodoItemDto>>, GetWithPaginationQueryHandler<TodoItem, TodoItemDto>>()
            .AddScoped<IRequestHandler<ExportQuery<TodoItem, TodoItemRecord>, ExportData>, ExportQueryHandler<TodoItem, TodoItemRecord>>();

    }

    private static IServiceCollection AddScopedMediatR<T>(this IServiceCollection services) where T : class, IEntity
    {
        services
            .AddScoped<IRequestHandler<FindByQuery<T>, IQueryable<T>>, FindByQueryHandler<T>>()
            .AddScoped<IRequestHandler<FindByQuery<T>, IQueryable<T>>, FindByQueryHandler<T>>()
            .AddScoped<IRequestHandler<CreateCommand<T>, int>, CreateCommandHandler<T>>()
            .AddScoped<IRequestHandler<UpdateCommand<T>, int>, UpdateCommandHandler<T>>()
            .AddScoped<IRequestHandler<DeleteCommand<T>, int>, DeleteCommandHandler<T>>();

        return services;
    }
}
