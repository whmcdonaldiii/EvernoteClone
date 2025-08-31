using NoteNest.Server.Data;
using NoteNest.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NoteNest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly EvernoteDbContext _context;

    public CategoriesController(EvernoteDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        // Check if category already exists
        if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
        {
            return Conflict("Category already exists");
        }

        category.Id = 0; // Ensure new category
        category.IsDefault = false; // Custom categories are not default
        category.CreatedAt = DateTime.UtcNow;

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        // Don't allow renaming default categories
        if (existingCategory.IsDefault && existingCategory.Name != category.Name)
        {
            return BadRequest("Cannot rename default categories");
        }

        // Check if new name already exists
        if (existingCategory.Name != category.Name && 
            await _context.Categories.AnyAsync(c => c.Name == category.Name))
        {
            return Conflict("Category name already exists");
        }

        existingCategory.Name = category.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Don't allow deleting default categories
        if (category.IsDefault)
        {
            return BadRequest("Cannot delete default categories");
        }

        // Update all notes in this category to "General"
        var notesInCategory = await _context.Notes.Where(n => n.Category == category.Name).ToListAsync();
        foreach (var note in notesInCategory)
        {
            note.Category = "General";
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}
