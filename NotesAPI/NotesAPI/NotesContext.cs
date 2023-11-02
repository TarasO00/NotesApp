using Microsoft.EntityFrameworkCore;

namespace NotesAPI;

public class NotesContext : DbContext
{
    public NotesContext(DbContextOptions options) : base(options) { }
    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
}

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<Tag> Tags { get; } = new();
}

public class Tag
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Color { get; set; }
}