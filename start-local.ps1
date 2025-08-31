# NoteNest Local Development Startup Script
# This script starts both the server and client applications

Write-Host "Starting NoteNest for local development..." -ForegroundColor Green

# Function to check if a port is in use
function Test-Port {
    param([int]$Port)
    try {
        $connection = New-Object System.Net.Sockets.TcpClient
        $connection.Connect("localhost", $Port)
        $connection.Close()
        return $true
    }
    catch {
        return $false
    }
}

# Check if ports are available
$serverPort = 7166
$clientPort = 7188

if (Test-Port $serverPort) {
    Write-Host "Warning: Port $serverPort is already in use. Server may not start properly." -ForegroundColor Yellow
}

if (Test-Port $clientPort) {
    Write-Host "Warning: Port $clientPort is already in use. Client may not start properly." -ForegroundColor Yellow
}

# Start the server in the background
Write-Host "Starting server on https://localhost:$serverPort..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\NoteNest.Server'; dotnet run --urls https://localhost:$serverPort" -WindowStyle Normal

# Wait a moment for server to start
Start-Sleep -Seconds 3

# Start the client
Write-Host "Starting client on https://localhost:$clientPort..." -ForegroundColor Cyan
Write-Host "Opening browser..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\NoteNest.Client'; dotnet run --urls https://localhost:$clientPort" -WindowStyle Normal

Write-Host "`nNoteNest is starting up!" -ForegroundColor Green
Write-Host "Server: http://localhost:$serverPort" -ForegroundColor White
Write-Host "Client: https://localhost:$clientPort" -ForegroundColor White
Write-Host "`nPress Ctrl+C to stop both applications" -ForegroundColor Yellow

# Keep the script running
try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
catch {
    Write-Host "`nShutting down..." -ForegroundColor Yellow
    # Kill any dotnet processes (be careful with this in production)
    Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
} 