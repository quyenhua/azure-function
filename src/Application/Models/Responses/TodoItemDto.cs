﻿using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.Responses;

public class TodoItemDto : IMapFrom<Todo>
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string Title { get; set; }

    public bool Done { get; set; }

    public int Priority { get; set; }

    public string Note { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Todo, TodoItemDto>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
    }
}