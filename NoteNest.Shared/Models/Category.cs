namespace NoteNest.Shared.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
