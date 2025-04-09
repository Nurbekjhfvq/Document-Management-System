namespace Document.Entity;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}

