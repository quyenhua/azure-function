using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<ListTodo>
{
    public void Configure(EntityTypeBuilder<ListTodo> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.Property(t => t.Title)
            .HasMaxLength(40)
            .IsRequired();
    }
}
