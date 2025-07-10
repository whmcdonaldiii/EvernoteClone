using EvernoteClone.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;

namespace EvernoteClone.Services;

public interface INoteService
{
    Task<List<Note>> GetAllNotesAsync();
    Task<Note?> GetNoteByIdAsync(int id);
    Task<Note> CreateNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(int id);
    Task<List<Note>> SearchNotesAsync(string searchTerm);
    Task<List<Category>> GetAllCategoriesAsync();
    Task<List<string>> GetAllTagsAsync();
    Task<Category> AddCustomCategoryAsync(string categoryName);
}

public class ApiNoteService : INoteService
{
    private readonly HttpClient _httpClient;
    private readonly IToastService _toastService;
    private readonly ILocalStorageService _localStorage;
    private const string NotesKey = "evernote_notes";
    private const string CategoriesKey = "evernote_categories";

    public ApiNoteService(HttpClient httpClient, IToastService toastService, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _localStorage = localStorage;
    }

    public async Task<List<Note>> GetAllNotesAsync()
    {
        try
        {
            var notes = await _httpClient.GetFromJsonAsync<List<Note>>("api/notes");
            if (notes != null)
            {
                await _localStorage.SetItemAsync(NotesKey, notes);
                return notes;
            }
            return new List<Note>();
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("offline") || ex.Message.Contains("network"))
        {
            _toastService.ShowWarning("Unable to load notes - working offline");
            return await GetOfflineNotes();
        }
        catch (Exception)
        {
            _toastService.ShowError("Failed to load notes");
            return await GetOfflineNotes();
        }
    }

    public async Task<Note?> GetNoteByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Note>($"api/notes/{id}");
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting note {id}: {ex.Message}");
            var offlineNotes = await GetOfflineNotes();
            return offlineNotes.FirstOrDefault(n => n.Id == id);
        }
    }

    public async Task<Note> CreateNoteAsync(Note note)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/notes", note);
            response.EnsureSuccessStatusCode();
            var createdNote = await response.Content.ReadFromJsonAsync<Note>();
            _toastService.ShowSuccess("Note created successfully!");
            return createdNote ?? note;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("offline") || ex.Message.Contains("network"))
        {
            _toastService.ShowError("Unable to create note - check your internet connection");
            throw;
        }
        catch (Exception ex)
        {
            _toastService.ShowError("Failed to create note");
            Console.WriteLine($"ERROR creating note: {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    public async Task<Note> UpdateNoteAsync(Note note)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notes/{note.Id}", note);
            response.EnsureSuccessStatusCode();
            _toastService.ShowSuccess("Note updated successfully!");
            return note;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("offline") || ex.Message.Contains("network"))
        {
            _toastService.ShowError("Unable to save changes - check your internet connection");
            throw;
        }
        catch (Exception ex)
        {
            _toastService.ShowError("Failed to update note");
            Console.WriteLine($"Error updating note: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteNoteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/notes/{id}");
            response.EnsureSuccessStatusCode();
            _toastService.ShowSuccess("Note deleted successfully!");
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("offline") || ex.Message.Contains("network"))
        {
            _toastService.ShowError("Unable to delete note - check your internet connection");
            throw;
        }
        catch (Exception ex)
        {
            _toastService.ShowError("Failed to delete note");
            Console.WriteLine($"Error deleting note {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Note>> SearchNotesAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllNotesAsync();

            var notes = await _httpClient.GetFromJsonAsync<List<Note>>($"api/notes/search?query={Uri.EscapeDataString(searchTerm)}");
            return notes ?? new List<Note>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching notes: {ex.Message}");
            var offlineNotes = await GetOfflineNotes();
            return offlineNotes.Where(n => 
                n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                n.Tags.Any(t => t.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>("api/categories");
            if (categories != null)
            {
                await _localStorage.SetItemAsync(CategoriesKey, categories);
                return categories;
            }
            return new List<Category>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR getting categories: {ex.GetType().Name}: {ex.Message}");
            return await GetOfflineCategories();
        }
    }

    public async Task<Category> AddCustomCategoryAsync(string categoryName)
    {
        try
        {
            var category = new Category { Name = categoryName.Trim() };
            var response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();
            var createdCategory = await response.Content.ReadFromJsonAsync<Category>();
            _toastService.ShowSuccess($"Category '{categoryName}' added successfully!");
            return createdCategory ?? category;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("offline") || ex.Message.Contains("network"))
        {
            _toastService.ShowError("Unable to add category - check your internet connection");
            throw;
        }
        catch (Exception ex)
        {
            _toastService.ShowError("Failed to add category");
            Console.WriteLine($"Error adding category: {ex.Message}");
            throw;
        }
    }

    public async Task<List<string>> GetAllTagsAsync()
    {
        try
        {
            var notes = await GetAllNotesAsync();
            return notes.SelectMany(n => n.Tags).Distinct().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting tags: {ex.Message}");
            var offlineNotes = await GetOfflineNotes();
            return offlineNotes.SelectMany(n => n.Tags).Distinct().ToList();
        }
    }

    private async Task<List<Note>> GetOfflineNotes()
    {
        try
        {
            return await _localStorage.GetItemAsync<List<Note>>(NotesKey) ?? new List<Note>();
        }
        catch
        {
            return new List<Note>();
        }
    }

    private async Task<List<Category>> GetOfflineCategories()
    {
        try
        {
            return await _localStorage.GetItemAsync<List<Category>>(CategoriesKey) ?? new List<Category>();
        }
        catch
        {
            return new List<Category>();
        }
    }
}
