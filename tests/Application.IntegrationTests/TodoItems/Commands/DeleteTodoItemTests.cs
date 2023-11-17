using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using Application.Features.Generics.Commands;

namespace Application.Features.IntegrationTests.TodoItems.Commands
{
    using static Testing;

    public class DeleteTodoItemTests : TestBase
    {
        [Test]
        public void ShouldRequireValidTodoItemId()
        {
            var command = new DeleteCommand<Todo> { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoItem()
        {
            var listId = await SendAsync(new CreateCommand<ListTodo>
            {
                Entity = new ListTodo
                {
                    Title = "New List"
                }
            });

            var itemId = await SendAsync(new CreateCommand<Todo>
            {
                Entity = new Todo
                {
                    ListId = listId,
                    Title = "New Item"
                }
            });

            await SendAsync(new DeleteCommand<Todo>
            {
                Id = itemId
            });

            var list = await FindAsync<Todo>(listId);

            list.Should().BeNull();
        }
    }
}
