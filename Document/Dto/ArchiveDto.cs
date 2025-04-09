namespace Document.Dto;

public class ArchiveDto
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public string ArchivePath { get; set; } = null!;
    public DateTime ArchivedAt { get; set; }
}
