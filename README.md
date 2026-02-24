# microsoft-contact-management

A simple cli contact management system written in C#. It uses JSON for storage. 
Supports CRUD operations, searching, filtering, and saving contacts.

---
## Features
- Add, edit, and delete contacts
- List all contacts
- View contact
- Search contacts by keyword (name, email, and phone)
- Filter contacts by name, email, phone, and creation date
- Save and load contacts to/from a JSON file
- Async operations for non-blocking IO

## How to Run
- You need to have .NET 10 SDK

1. Clone the repository
```bash
git clone https://github.com/HassanIsmail16/microsoft-contact-management.git
cd microsoft-contact-management
```

2. Build the project
```bash
dotnet build
```

3. Run the application
```bash
dotnet run
```

4. Use the Application

## Notes
- Contacts are stored in a JSON file `contacts.json` in the same directory as the executable
- This file is automatically loaded, or created if not found, on startup
- Input validation is purposefully minimal for email and phone number to make development and testing easier