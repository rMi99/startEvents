# StarEvents - Installation Guide

## Overview
StarEvents is a comprehensive online event booking system built with ASP.NET Core 8.0, featuring role-based dashboards for Administrators, Event Organizers, and Customers.

## Prerequisites

### Required Software
1. **Visual Studio 2022** (Community or higher) or **Visual Studio Code**
2. **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
3. **MySQL Server 8.0+** - [Download here](https://dev.mysql.com/downloads/mysql/)
4. **Git** - [Download here](https://git-scm.com/downloads)

### System Requirements
- **Operating System**: Windows 10/11, macOS, or Linux
- **RAM**: Minimum 4GB (8GB recommended)
- **Storage**: At least 2GB free space
- **Network**: Internet connection for package downloads

## Installation Steps

### 1. Clone the Repository
```bash
git clone https://github.com/rMi99/startEvents.git
cd startEvents
```

### 2. Database Setup

#### Option A: Using MySQL Workbench (Recommended)
1. Open MySQL Workbench
2. Connect to your MySQL server (usually `localhost:3306`)
3. Create a new database:
   ```sql
   CREATE DATABASE EventBookingDB;
   ```
4. Import the database dump:
   - Go to **Server** → **Data Import**
   - Select **Import from Self-Contained File**
   - Browse and select `Database/dump-eventbookingdb-202508241021.sql`
   - Set **Default Target Schema** to `EventBookingDB`
   - Click **Start Import**

#### Option B: Using Command Line
```bash
# Create database
mysql -u root -p -e "CREATE DATABASE EventBookingDB;"

# Import database dump
mysql -u root -p EventBookingDB < Database/dump-eventbookingdb-202508241021.sql
```

### 3. Configure Database Connection
1. Open `appsettings.json`
2. Update the connection string with your MySQL credentials:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;port=3306;database=EventBookingDB;user=YOUR_USERNAME;password=YOUR_PASSWORD;AllowZeroDateTime=True;ConvertZeroDateTime=True"
     }
   }
   ```
3. For development, you can also update `appsettings.Development.json`

### 4. Restore NuGet Packages
```bash
dotnet restore
```

### 5. Apply Database Migrations (if needed)
```bash
dotnet ef database update
```

### 6. Build the Application
```bash
dotnet build
```

## Running the Application

### Development Mode
```bash
dotnet run
```

The application will be available at:
- **HTTP**: http://localhost:5105
- **HTTPS**: https://localhost:7105 (if SSL is configured)

### Production Mode
```bash
dotnet run --environment Production
```

## Project Dependencies

### Core Framework
- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core 8.0** - ORM for database operations
- **ASP.NET Core Identity** - Authentication and authorization

### Database
- **Pomelo.EntityFrameworkCore.MySql 8.0.0** - MySQL provider for EF Core
- **Microsoft.EntityFrameworkCore.SqlServer 8.0.13** - SQL Server support (optional)
- **Microsoft.EntityFrameworkCore.Sqlite 8.0.11** - SQLite support (optional)

### Development Tools
- **Microsoft.EntityFrameworkCore.Tools 8.0.13** - EF Core CLI tools
- **Microsoft.VisualStudio.Web.CodeGeneration.Design 8.0.7** - Code scaffolding
- **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 8.0.13** - EF diagnostics

### Frontend Libraries (CDN)
- **Bootstrap 5.3.0** - CSS framework
- **Font Awesome 6.4.0** - Icons
- **jQuery 3.6.0** - JavaScript library

## Default User Accounts

After database import, the following default accounts are available:

### Administrator
- **Email**: admin@starlevents.com
- **Password**: Password@123
- **Role**: Admin

### Event Organizer
- **Email**: organizer@starlevents.com
- **Password**: Password@123
- **Role**: Organizer

### Customer
- **Email**: customer@starlevents.com
- **Password**: Password@123
- **Role**: Customer

## Features Overview

### Role-Based Dashboards
1. **Admin Dashboard**
   - User management
   - Event oversight
   - System reports
   - Application settings

2. **Organizer Dashboard**
   - Create and manage events
   - Ticket sales tracking
   - Event analytics
   - Revenue management

3. **Customer Dashboard**
   - Browse and book events
   - Manage tickets
   - Loyalty points system
   - Profile management

## Troubleshooting

### Common Issues

#### 1. Database Connection Failed
- Verify MySQL server is running
- Check connection string credentials
- Ensure database `EventBookingDB` exists
- Verify MySQL port (default: 3306)

#### 2. Migration Errors
```bash
# Reset migrations (caution: will lose data)
dotnet ef database drop --force
dotnet ef database update
```

#### 3. Package Restore Issues
```bash
# Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore --force
```

#### 4. Port Already in Use
- Change port in `Properties/launchSettings.json`
- Or kill the process using the port:
  ```bash
  # Windows
  netstat -ano | findstr :5105
  taskkill /PID <PID> /F
  
  # Linux/macOS
  lsof -ti:5105 | xargs kill
  ```

### Logging
- Application logs are written to the console during development
- For production, configure logging in `appsettings.Production.json`

## Development Environment Setup

### Visual Studio 2022
1. Open `online-event-booking.sln`
2. Set startup project to `online-event-booking`
3. Press F5 to run with debugging

### Visual Studio Code
1. Install C# extension
2. Open the project folder
3. Use `Ctrl+F5` to run without debugging

### Database Management
- Use MySQL Workbench for database administration
- Entity Framework migrations for schema changes
- Seed data is automatically applied on startup

## Security Notes
- Change default passwords in production
- Update connection strings for production database
- Configure HTTPS certificates for production
- Review and update CORS policies if needed

## Support
For issues and questions:
1. Check the troubleshooting section above
2. Review application logs
3. Ensure all prerequisites are properly installed
4. Verify database connectivity

## Next Steps
After successful installation:
1. Log in with default accounts to explore features
2. Create test events as an organizer
3. Test booking process as a customer
4. Review admin dashboard functionality
5. Customize themes and branding as needed

---

**Last Updated**: September 2025
**Version**: 1.0.0
**Framework**: ASP.NET Core 8.0
