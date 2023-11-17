using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Application.Common.Interfaces;

namespace Infrastructure.Services.BlobStorage;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient = new(new Uri("http://azurefuncmystorage.blob.core.windows.net"));

    public async Task UploadBlobFromStream(string container, string blobName, Stream stream)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(blobName);
            var transferOptions = new StorageTransferOptions
            {
                // Set the maximum number of parallel transfer workers
                MaximumConcurrency = 2,

                // Set the initial transfer length to 8 MiB
                InitialTransferSize = 8 * 1024 * 1024,

                // Set the maximum length of a transfer to 4 MiB
                MaximumTransferSize = 4 * 1024 * 1024
            };

            var validationOptions = new UploadTransferValidationOptions
            {
                ChecksumAlgorithm = StorageChecksumAlgorithm.Auto
            };

            var uploadOptions = new BlobUploadOptions()
            {
                TransferOptions = transferOptions,
                TransferValidation = validationOptions
            };

            await blobClient.UploadAsync(stream, uploadOptions);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
