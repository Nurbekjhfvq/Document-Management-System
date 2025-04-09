namespace Document.Entity;

public class FileDocument
{
    public int Id { get; set; }

    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public string FileName { get; set; } = null!;     // Foydalanuvchi bergan nom
    public string FilePath { get; set; } = null!;     // Local path: /files/groupA/fayl.pdf
    public string FileType { get; set; } = null!;
    public long Size { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

