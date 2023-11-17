using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ListTodo> TodoLists { get; set; }

    DbSet<Todo> TodoItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
