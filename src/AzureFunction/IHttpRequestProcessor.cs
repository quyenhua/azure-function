using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp;

public interface IHttpRequestProcessor
{
    Task<HttpResponseData> ExecuteAsync<TRequest, TResponse>(FunctionContext executionContext,
                                                                            HttpRequestData httpRequest,
                                                                            TRequest request,
                                                                            Func<TResponse, Task<HttpResponseData>>? resultMethod = null)
                                                                            where TRequest : IRequest<TResponse>;
}