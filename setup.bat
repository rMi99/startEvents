@echo off
echo ==========================================
echo        StarEvents - Quick Setup Script
echo ==========================================
echo.

echo [1/6] Checking .NET 8.0 SDK...
dotnet --version 2>nul
if %errorlevel% neq 0 (
    echo ERROR: .NET 8.0 SDK not found. Please install from https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)
echo .NET SDK found!
echo.

echo [2/6] Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo.

echo [3/6] Building the application...
dotnet build
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)
echo.

echo [4/6] Checking database connection...
echo Please ensure MySQL server is running and database 'EventBookingDB' exists.
echo If not, please run MySQL Workbench and import Database/dump-eventbookingdb-202508241021.sql
echo.
pause

echo [5/6] Applying database migrations...
dotnet ef database update
if %errorlevel% neq 0 (
    echo WARNING: Migration failed. This might be normal if database is already set up.
)
echo.

echo [6/6] Setup complete!
echo.
echo ==========================================
echo        Ready to run StarEvents!
echo ==========================================
echo.
echo To start the application, run: dotnet run
echo Application will be available at: http://localhost:5105
echo.
echo Default login accounts:
echo - Admin: admin@starlevents.com / Admin@123
echo - Organizer: organizer@starlevents.com / Organizer@123  
echo - Customer: customer@starlevents.com / Customer@123
echo.
echo Press any key to start the application now...
pause

echo Starting application...
dotnet run
