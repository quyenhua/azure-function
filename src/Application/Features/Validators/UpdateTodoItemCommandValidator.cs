using Application.Features.Generics.Commands;
using Domain.Entities;

using FluentValidation;

namespace Application.Features.Validators;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateCommand<Todo>>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(v => v.Entity.Title)
            .MaximumLength(40)
            .NotEmpty();
    }
}
