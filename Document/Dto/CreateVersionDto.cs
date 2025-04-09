namespace Document.Dto;

public class CreateVersionDto
{
    public int DocumentId { get; set; }
    public byte[] FileContent { get; set; } = null!;
    public string FileType { get; set; } = null!;
}