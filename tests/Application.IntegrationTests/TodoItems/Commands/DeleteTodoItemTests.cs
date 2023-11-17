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
            var command = new DeleteCommand<TodoItem> { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoItem()
        {
            var listId = await SendAsync(new CreateCommand<TodoList>
            {
                Entity = new TodoList
                {
                    Title = "New List"
                }
            });

            var itemId = await SendAsync(new CreateCommand<TodoItem>
            {
                Entity = new TodoItem
                {
                    ListId = listId,
                    Title = "New Item"
                }
            });

            await SendAsync(new DeleteCommand<TodoItem>
            {
                Id = itemId
            });

            var list = await FindAsync<TodoItem>(listId);

            list.Should().BeNull();
        }
    }
}
