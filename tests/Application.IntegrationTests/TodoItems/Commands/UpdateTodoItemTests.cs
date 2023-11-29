using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using Application.Features.Generics.Commands;

namespace Application.Features.IntegrationTests.TodoItems.Commands
{
    using static Testing;

    public class UpdateTodoItemTests : TestBase
    {
        //[Test]
        //public void ShouldRequireValidTodoItemId()
        //{
        //    var command = new UpdateCommand<Todo>
        //    {
        //        Entity = new Todo
        //        {
        //            Id = 99,
        //            Title = "New Title"
        //        }
        //    };

        //    FluentActions.Invoking(() =>
        //        SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        //}

        //[Test]
        //public async Task ShouldUpdateTodoItem()
        //{
        //    var userId = await RunAsDefaultUserAsync();

        //    var listId = await SendAsync(new CreateCommand<ListTodo>
        //    {
        //        Entity = new ListTodo
        //        {
        //            Title = "New List"
        //        }
        //    });

        //    var itemId = await SendAsync(new CreateCommand<Todo>
        //    {
        //        Entity = new Todo
        //        {
        //            ListId = listId,
        //            Title = "New Item"
        //        }
        //    });

        //    var command = new UpdateCommand<Todo>
        //    {
        //        Entity = new Todo
        //        {
        //            ListId = listId,
        //            Title = "Updated New Item"
        //        }
        //    };

        //    await SendAsync(command);

        //    var item = await FindAsync<Todo>(itemId);

        //    item.Title.Should().Be(command.Entity.Title);
        //    item.LastModifiedBy.Should().NotBeNull();
        //    item.LastModifiedBy.Should().Be(userId);
        //    item.LastModified.Should().NotBeNull();
        //    item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1000));
        //}
    }
}
