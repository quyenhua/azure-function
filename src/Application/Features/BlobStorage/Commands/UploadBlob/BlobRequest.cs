namespace Application.Features.BlobStorage.Commands;

public class BlobRequest
{
    public string Container { get; set; } = "blobcontainer";

    public Stream File { get; set; }

    public string FileName { get; set; } = "test";
}
