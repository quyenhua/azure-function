using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using Application.Features.WeatherForecasts.Queries.GetWeatherForecasts;
using Application.Features.BlobStorage.Commands;

namespace FunctionApp
{
    public class WeatherForecastsFunctions(IHttpRequestProcessor processor)
    {
        private readonly IHttpRequestProcessor _processor = processor;

        [Function(nameof(GetWeatherForecasts))]
        public async Task<HttpResponseData> GetWeatherForecasts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "weather/forecasts")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<WeatherForecastsFunctions>();
            logger.LogInformation("Called GetWeatherForecasts");

            return await _processor.ExecuteAsync<GetWeatherForecastsQuery, IEnumerable<WeatherForecast>>(executionContext,
                                                                req,
                                                                new GetWeatherForecastsQuery(),
                                                                (r) => req.CreateObjectResponseAsync(r));
        }

        [Function(nameof(UploadBlobContainer))]
        public async Task<HttpResponseData> UploadBlobContainer([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "blob")]
        HttpRequestData req,
        FunctionContext functionContext)
        {
            var request = new UploadBlobContainer
            {
                Blob = new BlobRequest
                {
                    File = GenerateStreamFromString(req.ReadAsString())
                }
            };

            return await _processor.ExecuteAsync<UploadBlobContainer, Unit>(functionContext,
                                                                req,
                                                                request,
                                                                (r) => req.CreateResponseAsync());
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}