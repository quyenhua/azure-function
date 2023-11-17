using Application.Features.Generics.Commands;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Application.Features.IntegrationTests.TodoItems.Commands
{
    using static Testing;

    public class CreateTodoItemTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateCommand<Todo> { Entity = new Todo
            {
                Title = "Title"
            } };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateTodoItem()
        {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateCommand<Todo>
            {
                Entity = new Todo
                {
                    Title = "New List"
                }
            });

            var command = new CreateCommand<Todo>()
            {
                Entity = new Todo
                {
                    ListId = listId,
                    Title = "Tasks"
                }
            };

            var itemId = await SendAsync(command);

            var item = await FindAsync<Todo>(itemId);

            item.Should().NotBeNull();
            item.ListId.Should().Be(command.Entity.ListId);
            item.Title.Should().Be(command.Entity.Title);
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10000));
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }
    }
}
