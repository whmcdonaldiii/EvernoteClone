@echo off
echo Starting EvernoteClone for local development...
echo.

REM Check if .NET is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo Error: .NET 9 SDK is not installed or not in PATH
    echo Please install .NET 9 SDK from https://dotnet.microsoft.com/download/dotnet/9.0
    pause
    exit /b 1
)

echo Starting server on http://localhost:5059...
start "EvernoteClone Server" cmd /k "cd /d %~dp0EvernoteClone.Server && dotnet run --urls http://localhost:5059"

REM Wait for server to start
timeout /t 3 /nobreak >nul

echo Starting client on http://localhost:5194...
start "EvernoteClone Client" cmd /k "cd /d %~dp0EvernoteClone.Client && dotnet run --urls http://localhost:5194"

echo.
echo EvernoteClone is starting up!
echo Server: http://localhost:5059
echo Client: http://localhost:5194
echo.
echo The application will open in your browser automatically.
echo Close the command windows to stop the applications.
echo.
pause 