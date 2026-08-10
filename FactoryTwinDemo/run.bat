@echo off
cd /d "%~dp0"
dotnet run --project FactoryTwinDemo.csproj
if errorlevel 1 (
  echo.
  echo Failed to start. Please install the .NET 9 SDK first.
  pause
)
