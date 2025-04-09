namespace Document.Dto;

public class VersionDto
{
    public int Id { get; set; }
    public int DocumentId { get; set; }

    public byte[] FileContent { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
}
