using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using Application.Features.Generics.Queries;
using Application.Features.Generics.Commands;
using Domain.Entities;
using Domain.ValueObjects;

namespace FunctionApp;

public class TodoListFunctions(IHttpRequestProcessor processor, ILogger<TodoListFunctions> logger)
{
    private readonly IHttpRequestProcessor _processor = processor;
    private readonly ILogger<TodoListFunctions> logger = logger;

    [Function(nameof(GetTodos))]
    public async Task<HttpResponseData> GetTodos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todolists")]
        HttpRequestData req,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called GetTodos");

        return await _processor.ExecuteAsync<FindByQuery<ListTodo>, IQueryable<ListTodo>>(functionContext,
                                                            req,
                                                            new FindByQuery<ListTodo> { Predicate = _ => true },
                                                            (r) => req.CreateObjectResponseAsync(r));
    }

    [Function(nameof(CreateTodosList))]
    public async Task<HttpResponseData> CreateTodosList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todolists")]
        HttpRequestData req,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called CreateTodosList");

        var todoList = await req.ReadFromJsonAsync<TodoListRes>();
        var command = new CreateCommand<ListTodo>
        {
            Entity = new ListTodo
            {
                Title = "Todo list",
                Color = Colors.Blue
            }
        };

        return await _processor.ExecuteAsync<CreateCommand<ListTodo>, int>(functionContext,
                                                            req,
                                                            command,
                                                            (r) => req.CreateObjectCreatedResponseAsync("todolists", r));
    }

    [Function(nameof(UpdateTodosList))]
    public async Task<HttpResponseData> UpdateTodosList([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todolists/{id}")]
        HttpRequestData req,
        int id,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called UpdateTodosList");

        var todoList = await req.ReadFromJsonAsync<TodoListRes>();
        var request = new UpdateCommand<ListTodo>
        {
            Entity = new ListTodo
            {
                Id = id,
                Title = todoList.Title
            }
        };

        return await _processor.ExecuteAsync<UpdateCommand<ListTodo>, int>(functionContext,
                                                            req,
                                                            request,
                                                            (r) => req.CreateResponseAsync());
    }

    [Function(nameof(DeleteTodosList))]
    public async Task<HttpResponseData> DeleteTodosList([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todolists/{id}")]
        HttpRequestData req,
        int id,
        FunctionContext functionContext)
    {
        logger.LogInformation("Called DeleteTodosList");

        var request = new DeleteCommand<ListTodo>
        {
            Id = id
        };

        return await _processor.ExecuteAsync<DeleteCommand<ListTodo>, int>(functionContext,
                                                            req,
                                                            request,
                                                            (r) => req.CreateResponseAsync(System.Net.HttpStatusCode.NoContent));
    }
}

public class TodoListRes
{
    public string Title { get; set; }
}