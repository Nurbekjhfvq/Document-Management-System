namespace Document.Entity
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public long Size { get; set; }
        public byte[]? FileContent { get; set; } // Agar file deb kiritilsa, FileContent bo'lmaydi
        public string FileType { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastAccessedDate { get; set; }

        public bool IsArchived { get; set; } = false;

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Version> Versions { get; set; } = new List<Version>();
        public virtual ICollection<Archive> Archives { get; set; } = new List<Archive>();

        // Yangi polya qo'shamiz: IsFile
        public bool IsFile { get; set; } = false; // Agar true bo'lsa, bu fayl (file deb kiritilgan) bo'ladi
    }
}
