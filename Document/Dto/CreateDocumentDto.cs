namespace Document.Dto
{
    public class CreateDocumentDto
    {
        public string Name { get; set; } = null!;
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public bool IsFile { get; set; } // Faylni serverga yoki databasega saqlashni tanlash
        public IFormFile File { get; set; } = null!;
    }
}
