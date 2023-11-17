using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using Application.Common.Models;
using Application.Features.Generics.Queries;
using Application.Models.Responses;
using Application.Features.Generics.Commands;

using Domain.Entities;

namespace FunctionApp;

public class TodoItemsFunctions(IHttpRequestProcessor processor, ILogger<TodoItemsFunctions> logger)
{
    private readonly IHttpRequestProcessor _processor = processor;
    private readonly ILogger<TodoItemsFunctions> logger = logger;

    [Function(nameof(GetTodoItems))]
    public async Task<HttpResponseData> GetTodoItems([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todolists/{id}")]
        HttpRequestData req,
        int id,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called GetTodoItems");

        if (req.Headers.TryGetValues("Content-Type", out IEnumerable<string> values))
        {
            if (values.Contains("text/csv"))
            {
                var request = new ExportQuery<TodoItem, TodoItemRecord>
                {
                    Predicate = _ => true,
                    FileName = "TodoItems"
                };
                return await _processor.ExecuteAsync<ExportQuery<TodoItem, TodoItemRecord>, ExportData>(functionContext,
                                                                    req,
                                                                    request,
                                                                    (r) => req.CreateFileContentResponseAsync(r.Content, r.ContentType, r.FileName));
            }
        }

        var query = new GetWithPaginationQuery<TodoItem, TodoItemDto>
        {
            Predicate = _ => true
        };

        return await _processor.ExecuteAsync<GetWithPaginationQuery<TodoItem, TodoItemDto>, PaginatedList<TodoItemDto>>(functionContext,
                                                            req,
                                                            query,
                                                            (r) => req.CreateObjectResponseAsync(r));
    }

    [Function(nameof(CreateTodoItem))]
    public async Task<HttpResponseData> CreateTodoItem([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todolists/{id}/items")]
        HttpRequestData req,
        int id,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called CreateTodoItems");

        var todoList = await req.ReadFromJsonAsync<TodoListRes>();
        var command = new CreateCommand<TodoItem>
        {
            Entity = new TodoItem { Id = id, Title = "Todo item" },
        };

        return await _processor.ExecuteAsync<CreateCommand<TodoItem>, int>(functionContext,
                                                            req,
                                                            command,
                                                            (r) => req.CreateObjectCreatedResponseAsync($"todolists/{id}/items", r));
    }
}