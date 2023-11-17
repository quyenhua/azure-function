namespace Application.Models.Responses;

public class ExportData
{
    public string FileName { get; set; }

    public string ContentType { get; set; }

    public byte[] Content { get; set; }
}