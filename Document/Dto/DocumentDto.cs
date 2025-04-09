namespace Document.Dto;
public class DocumentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    // FilePath olib tashlandi
    public long Size { get; set; }
    public string FileType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsArchived { get; set; }

    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; } = null!;
}
