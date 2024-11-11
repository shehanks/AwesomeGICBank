# AwesomeGICBank

## Overview
AwesomeGICBank is a console application built with .NET 8 and Clean Architecture. It provides banking features such as account management, transactions, and interest calculations.

## Prerequisites
- **SQL Server** (latest version recommended)
- **Visual Studio 2022** (or any IDE supporting .NET 8)
- **.NET 8 SDK**

## Getting Started

### 1. Clone the Repository
Clone the project from GitHub:
```bash
git clone https://github.com/shehanks/AwesomeGICBank.git
```

### 2. Configure Database Connection
- Open appsettings.json in the AwesomeGICBank.CLI project
- Replace the DefaultConnection string with your SQL Server instance
  
For local SQL Server instance with Windows authentication:
  ```json
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AwesomeGICBank;Trusted_Connection=True;TrustServerCertificate=True"
  ```
For remote SQL Server with credentials:
```json
  "DefaultConnection": "Server=your_server;Database=AwesomeGICBank;User Id=your_username;Password=your_password;TrustServerCertificate=True"
  ```

## 3. Run the Application
Once the connection string is set, run the project from the AwesomeGICBank.CLI (the startup project), 
and it will automatically create the database using migration scripts.


## 4. Test the Application
You can now test the banking features provided by AwesomeGICBank through the console.

