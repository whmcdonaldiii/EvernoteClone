using NoteNest.Server.Data;
using NoteNest.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NoteNest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly EvernoteDbContext _context;

    public NotesController(EvernoteDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        return await _context.Notes.OrderByDescending(n => n.UpdatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        var note = await _context.Notes.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return note;
    }

    [HttpPost]
    public async Task<ActionResult<Note>> CreateNote(Note note)
    {
        note.Id = 0; // Ensure new note
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, Note note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        var existingNote = await _context.Notes.FindAsync(id);
        if (existingNote == null)
        {
            return NotFound();
        }

        existingNote.Title = note.Title;
        existingNote.Content = note.Content;
        existingNote.Category = note.Category;
        existingNote.Tags = note.Tags;
        existingNote.IsFavorite = note.IsFavorite;
        existingNote.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NoteExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Note>>> SearchNotes([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return await GetNotes();
        }

        var notes = await _context.Notes
            .Where(n => n.Title.Contains(query) || n.Content.Contains(query) || n.Tags.Any(t => t.Contains(query)))
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync();

        return notes;
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotesByCategory(string category)
    {
        var notes = await _context.Notes
            .Where(n => n.Category == category)
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync();

        return notes;
    }

    private bool NoteExists(int id)
    {
        return _context.Notes.Any(e => e.Id == id);
    }
}
