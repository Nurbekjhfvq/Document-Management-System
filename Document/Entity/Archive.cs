namespace Document.Entity;

public class Archive
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public virtual Document Document { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public DateTime? ArchivedAt { get; set; }
}
