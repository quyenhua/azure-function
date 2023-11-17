﻿using FluentValidation;

using Domain.Entities;
using Application.Features.Generics.Commands;

namespace Application.Features.Validators;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateCommand<TodoItem>>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Entity.Title)
            .MaximumLength(40)
            .NotEmpty();
    }
}
