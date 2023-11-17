using FluentValidation;

using Domain.Entities;
using Application.Features.Generics.Queries;
using Application.Models.Responses;

namespace Application.Features.Validators;

public class GetTodoItemsWithPaginationQueryValidator : AbstractValidator<GetWithPaginationQuery<Todo, TodoItemDto>>
{
    public GetTodoItemsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
