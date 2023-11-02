using Microsoft.EntityFrameworkCore;
using NotesAPI;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Notes") ?? "Data Source=Notes.db";
// builder.Services.AddDbContext<NotesContext>(options => options.UseSqlite(connectionString));
builder.Services.AddSqlite<NotesContext>(connectionString);

// Add services to the container.

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapGet("/notes", async (NotesContext db) => await db.Notes.Include(note => note.Tags).ToListAsync());
app.MapGet("/notes/{id}", async (NotesContext db, int id) => await db.Notes.Include(note => note.Tags).FirstAsync(n => n.Id == id));
app.MapPost("/notes", async (NotesContext db, Note note) =>
{
    note.CreatedAt = DateTime.Now;
    await db.Notes.AddAsync(note);
    await db.SaveChangesAsync();
    return Results.Created($"/notes/{note.Id}", note);
});
app.MapPut("/notes/{id}", async (NotesContext db, Note updatedNote, int id) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note is null) return Results.NotFound();
    note.Title = updatedNote.Title;
    note.Text = updatedNote.Text;
    note.UpdatedAt = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/notes/{id}", async (NotesContext db, int id) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note is null)
    {
        return Results.NotFound();
    }
    db.Notes.Remove(note);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapPost("/notes/{id}/tag/", async (NotesContext db, int id, int tagId) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note is null)
    {
        return Results.NotFound();
    }
    var tag = await db.Tags.FindAsync(tagId);
    if (tag is null)
    {
        return Results.NotFound();
    }
    note.Tags.Add(tag);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapDelete("/notes/{id}/tag/", async (NotesContext db, int id, int tagId) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note is null)
    {
        return Results.NotFound();
    }
    var tag = await db.Tags.FindAsync(tagId);
    if (tag is null)
    {
        return Results.NotFound();
    }
    note.Tags.Remove(tag);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/tags", async (NotesContext db) => await db.Tags.ToListAsync());
app.MapGet("/tags/{id}", async (NotesContext db, int id) => await db.Tags.FindAsync(id));
app.MapPost("/tags", async (NotesContext db, Tag tag) =>
{
    await db.Tags.AddAsync(tag);
    await db.SaveChangesAsync();
    return Results.Created($"/tags/{tag.Id}", tag);
});
app.MapPut("/tags/{id}", async (NotesContext db, Tag updatedTag, int id) =>
{
    var tag = await db.Tags.FindAsync(id);
    if (tag is null) return Results.NotFound();
    tag.Text = updatedTag.Text;
    tag.Color = updatedTag.Color;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/tags/{id}", async (NotesContext db, int id) =>
{
    var tag = await db.Tags.FindAsync(id);
    if (tag is null)
    {
        return Results.NotFound();
    }
    db.Tags.Remove(tag);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();