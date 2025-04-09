namespace Document.Dto;

public class DocumentFilterDto
{
    public long? MinSize { get; set; }     // Faylning minimal hajmi (baytda)
    public long? MaxSize { get; set; }     // Faylning maksimal hajmi (baytda)
    public string? FileType { get; set; }  // Fayl turi: ".pdf", ".jpg", ".doc" va h.k.
    public DateTime? CreatedFrom { get; set; } // Qaysi sanadan boshlab yaratilgan
    public DateTime? CreatedTo { get; set; }   // Qaysi sanagacha yaratilgan
}
