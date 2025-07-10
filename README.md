# EvernoteClone - Personal Note-Taking Application

A modern, feature-rich note-taking application built with Blazor WebAssembly and .NET 9, designed for personal use.

## Features

- 📝 **Rich Text Editor** - Create and edit notes with formatting
- 🏷️ **Categories & Tags** - Organize notes with categories and tags
- 🔍 **Search** - Find notes by title, content, or tags
- 💾 **Offline Support** - Works offline with local storage
- 📱 **Responsive Design** - Works on desktop and mobile
- 🎨 **Modern UI** - Clean, intuitive interface with Bootstrap
- ⚡ **Fast Performance** - Built with Blazor WebAssembly

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A modern web browser (Chrome, Firefox, Safari, Edge)

## Quick Start

### Option 1: Automated Startup (Recommended)

1. Open PowerShell in the project root directory
2. Run the startup script:
   ```powershell
   .\start-local.ps1
   ```
3. The application will automatically open in your browser at `http://localhost:5194`

### Option 2: Manual Startup

1. **Start the Server** (API Backend):
   ```bash
   cd EvernoteClone.Server
   dotnet run
   ```
   The server will start on `http://localhost:5059`

2. **Start the Client** (Blazor WebAssembly):
   ```bash
   cd EvernoteClone.Client
   dotnet run
   ```
   The client will start on `http://localhost:5194`

3. Open your browser and navigate to `http://localhost:5194`

## Project Structure

```
EvernoteClone/
├── EvernoteClone.Client/          # Blazor WebAssembly frontend
│   ├── Components/                # Reusable UI components
│   ├── Pages/                     # Application pages
│   ├── Services/                  # Client-side services
│   └── wwwroot/                   # Static assets
├── EvernoteClone.Server/          # .NET Web API backend
│   ├── Controllers/               # API endpoints
│   ├── Data/                      # Database context
│   └── Services/                  # Server-side services
├── EvernoteClone.Shared/          # Shared models and DTOs
└── EvernoteClone.Data/            # Data access layer
```

## Database

The application uses SQLite for data storage. The database file (`evernote.db`) is automatically created in the `EvernoteClone.Server` directory when you first run the application.

### Default Categories
- General
- Work
- Personal
- Ideas
- Tasks

## Usage

### Creating Notes
1. Click the "New Note" button in the sidebar
2. Enter a title and content
3. Select a category and add tags
4. Click the save button or press Ctrl+S

### Organizing Notes
- Use categories to group related notes
- Add tags for more detailed organization
- Use the search function to find specific notes
- Mark notes as favorites for quick access

### Offline Mode
The application works offline using browser local storage. Notes created while offline will be stored locally and can be synced when the connection is restored.

## Development

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Database Migrations
If you need to update the database schema:
```bash
cd EvernoteClone.Server
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Configuration

### Server Configuration
Edit `EvernoteClone.Server/appsettings.json` to modify:
- Database connection string
- CORS settings
- Logging configuration

### Client Configuration
The client is configured to connect to the server at `http://localhost:5059`. If you change the server port, update the `BaseAddress` in `EvernoteClone.Client/Program.cs`.

## Troubleshooting

### Port Already in Use
If you get a port conflict error:
1. Check if another application is using the port
2. Kill the process using the port or change the port in the configuration
3. Restart the application

### Database Issues
If you encounter database errors:
1. Delete the `evernote.db` file in the server directory
2. Restart the server - it will recreate the database with default data

### Build Errors
If you get build errors:
1. Ensure you have .NET 9 SDK installed
2. Run `dotnet restore` to restore packages
3. Clean and rebuild: `dotnet clean && dotnet build`

## Contributing

This is a personal project, but if you find bugs or have suggestions:
1. Create an issue describing the problem
2. Fork the repository
3. Create a feature branch
4. Submit a pull request

## License

This project is for personal use only.

## Acknowledgments

- Built with [Blazor WebAssembly](https://blazor.net/)
- UI components from [Radzen](https://www.radzen.com/)
- Icons from [Font Awesome](https://fontawesome.com/)
- Styling with [Bootstrap](https://getbootstrap.com/) 