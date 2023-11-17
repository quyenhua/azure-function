using MediatR;

using Application.Features.BlobStorage.Commands;
using Application.Common.Interfaces;

namespace Application.Features.WeatherForecasts.Queries.GetWeatherForecasts;

public class UploadBlobContainer : IRequest<Unit>
{
    public BlobRequest Blob { get; set; }
}

public class UploadBlobContainerHandler(IBlobStorageService blobStorageService) : IRequestHandler<UploadBlobContainer, Unit>
{
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task<Unit> Handle(UploadBlobContainer request, CancellationToken cancellationToken)
    {
        await _blobStorageService.UploadBlobFromStream(request.Blob.Container, request.Blob.FileName, request.Blob.File);
        return Unit.Value;
    }
}
