# Copilot Instructions for EvernoteClone

<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

## Project Overview
This is a Blazor WebAssembly application that serves as an Evernote clone for personal note-taking and organization.

## Architecture & Patterns
- **Frontend**: Blazor WebAssembly with C#
- **State Management**: Consider using Fluxor or simple service-based state management
- **Data Storage**: Initially browser localStorage, designed for future API integration
- **Styling**: Bootstrap 5 with custom CSS for modern UI

## Key Features to Implement
- Note creation, editing, and deletion
- Rich text editing capabilities
- Search and filtering functionality
- Categories/tags for organization
- Responsive design for desktop and mobile
- Local storage with future API sync capability

## Code Guidelines
- Follow C# naming conventions and best practices
- Use dependency injection for services
- Implement proper error handling and user feedback
- Write clean, maintainable, and well-documented code
- Use async/await patterns appropriately
- Implement proper component lifecycle management

## UI/UX Guidelines
- Modern, clean design similar to Evernote
- Intuitive navigation and user experience
- Responsive layout that works on all devices
- Proper loading states and user feedback
- Accessible components following WCAG guidelines

## Future Considerations
- Design components with API integration in mind
- Consider offline-first approach with sync capabilities
- Plan for authentication and user management
- Think about data migration strategies
