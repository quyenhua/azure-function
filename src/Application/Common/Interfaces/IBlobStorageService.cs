namespace Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task UploadBlobFromStream(string container, string blobName, Stream stream);
}
