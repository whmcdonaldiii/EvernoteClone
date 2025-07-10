# Development Guide

This guide provides tips and best practices for developing and maintaining your Evernote clone locally.

## Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022, VS Code, or Rider
- Git (for version control)

### First Time Setup
1. Clone or download the project
2. Open a terminal in the project root
3. Run `dotnet restore` to restore packages
4. Run `dotnet build` to ensure everything compiles
5. Use the startup scripts to run the application

## Development Workflow

### Starting Development
```bash
# Option 1: Use the startup script (recommended)
.\start-local.ps1

# Option 2: Manual startup
cd EvernoteClone.Server && dotnet run
# In another terminal:
cd EvernoteClone.Client && dotnet run
```

### Hot Reload
Both the client and server support hot reload:
- Make changes to `.razor` files in the client
- Make changes to `.cs` files in the server
- The application will automatically reload

### Database Changes
If you need to modify the database schema:

1. Update the models in `EvernoteClone.Shared/Models/`
2. Update the DbContext in `EvernoteClone.Server/Data/EvernoteDbContext.cs`
3. Create a migration:
   ```bash
   cd EvernoteClone.Server
   dotnet ef migrations add MigrationName
   dotnet ef database update
   ```

## Project Structure

### Client (Blazor WebAssembly)
- `Pages/` - Main application pages
- `Components/` - Reusable UI components
- `Services/` - Client-side services and API calls
- `Layout/` - Application layout components
- `wwwroot/` - Static assets (CSS, JS, images)

### Server (.NET Web API)
- `Controllers/` - API endpoints
- `Data/` - Database context and configuration
- `Services/` - Business logic services
- `Models/` - Server-side models (if different from shared)

### Shared
- `Models/` - Shared data models used by both client and server

## Key Features Implementation

### Rich Text Editor
The application uses Quill.js for rich text editing:
- Integration in `Components/RichTextEditor.razor`
- JavaScript interop in `wwwroot/js/quill-interop.js`
- Styling from CDN in `wwwroot/index.html`

### Offline Support
Implemented using Blazored.LocalStorage:
- Notes are cached locally when online
- Works offline with cached data
- Syncs when connection is restored

### Search Functionality
- Server-side search in `NotesController.SearchNotes()`
- Client-side fallback for offline mode
- Searches title, content, and tags

## Adding New Features

### Adding a New API Endpoint
1. Create or update a controller in `EvernoteClone.Server/Controllers/`
2. Add the corresponding service method in `EvernoteClone.Client/Services/`
3. Update the UI to use the new functionality

### Adding a New Page
1. Create a new `.razor` file in `EvernoteClone.Client/Pages/`
2. Add the `@page` directive with the route
3. Update navigation if needed

### Adding a New Component
1. Create a new `.razor` file in `EvernoteClone.Client/Components/`
2. Define parameters using `[Parameter]` attributes
3. Use the component in pages or other components

## Styling and UI

### CSS Framework
- Bootstrap 5 for responsive design
- Font Awesome for icons
- Custom CSS in `wwwroot/css/app.css`

### Component Styling
- Use Bootstrap classes for layout and styling
- Create custom CSS classes for specific components
- Follow the existing design patterns

## Testing

### Manual Testing
1. Test all CRUD operations (Create, Read, Update, Delete)
2. Test search functionality
3. Test offline mode
4. Test responsive design on different screen sizes

### Automated Testing (Future)
Consider adding:
- Unit tests for services
- Integration tests for API endpoints
- Component tests for Blazor components

## Performance Optimization

### Client-Side
- Use `@key` directive for lists
- Implement virtual scrolling for large note lists
- Optimize component rendering with `ShouldRender()`

### Server-Side
- Use async/await for database operations
- Implement pagination for large datasets
- Add caching where appropriate

## Deployment

### Local Production Build
```bash
# Build for production
dotnet publish -c Release

# The published files will be in:
# EvernoteClone.Server/bin/Release/net9.0/publish/
# EvernoteClone.Client/bin/Release/net9.0/publish/
```

### Database Backup
The SQLite database is in `EvernoteClone.Server/evernote.db`. Back it up regularly:
```bash
cp EvernoteClone.Server/evernote.db EvernoteClone.Server/evernote.db.backup
```

## Troubleshooting

### Common Issues

**Build Errors**
- Run `dotnet clean && dotnet restore && dotnet build`
- Check for missing NuGet packages
- Ensure .NET 9 SDK is installed

**Runtime Errors**
- Check browser console for JavaScript errors
- Check server logs for API errors
- Verify database file permissions

**Database Issues**
- Delete `evernote.db` to reset the database
- Check connection string in `appsettings.json`
- Ensure Entity Framework migrations are up to date

### Debugging Tips
- Use browser developer tools for client-side debugging
- Use Visual Studio debugger for server-side debugging
- Add logging to track application flow
- Use the browser's Network tab to monitor API calls

## Best Practices

### Code Organization
- Keep components small and focused
- Use dependency injection for services
- Follow C# naming conventions
- Add XML documentation for public APIs

### Security
- Validate all user inputs
- Use parameterized queries for database operations
- Implement proper authentication (future enhancement)
- Sanitize HTML content

### Performance
- Minimize API calls
- Use efficient data structures
- Implement proper error handling
- Monitor memory usage

## Future Enhancements

Consider adding these features:
- User authentication and authorization
- Note sharing and collaboration
- File attachments
- Note templates
- Export/import functionality
- Dark mode theme
- Keyboard shortcuts
- Note versioning
- Advanced search filters
- Note encryption 