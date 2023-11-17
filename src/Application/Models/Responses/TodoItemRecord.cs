using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Models.Responses;

public class TodoItemRecord : IMapFrom<Todo>
{
    public string Title { get; set; }

    public bool Done { get; set; }
}