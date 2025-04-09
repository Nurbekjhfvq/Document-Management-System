namespace Document.Entity;

public class Version
{
    public int Id { get; set; }

    public int DocumentId { get; set; }
    public virtual Document Document { get; set; } = null!;

    public byte[] FileContent { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public long Size { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
