using MicrosoftContactManagement.Models;
using MicrosoftContactManagement.Repositories;

namespace MicrosoftContactManagement;

public class ContactManagementApplication
{
    private readonly IContactRepository _contactRepository;
    private bool _running = true;
    private int _menuDelay = 1000;
    public ContactManagementApplication(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task RunAsync()
    {
        try
        {

            await _contactRepository.LoadAsync();

            Console.WriteLine("Contacts loaded.");

            var contacts = (await _contactRepository.GetContactsAsync()).ToList();

            if (contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    Console.WriteLine(contact);
                }
            }
            else
            {
                Console.WriteLine("No contacts found.");
            }

            while (_running)
            {
                DisplayMenu();
                var input = Console.ReadLine()?.Trim() ?? String.Empty;
                await HandleMenuInput(input);
                await Task.Delay(_menuDelay);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occured... {ex.Message}");
        }
    }

    private async Task HandleAddContact()
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Add Contact");
        Console.WriteLine(new string('-', 50));
        
        var name = ReadInput("Enter contact name:");
        var email = ReadInput("Enter contact email: ", isEmail: true);
        var phone = ReadInput("Enter contact phone number: ", isPhoneNumber: true);
        
        var contact = new Contact(name, phone, email);
        
        await _contactRepository.InsertContactAsync(contact);
        Console.WriteLine("Contact added.");
    }
    
    private async Task HandleEditContact()
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Edit Contact");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("Enter contact id:");
        var input = Console.ReadLine()?.Trim();
        if (!Int32.TryParse(input, out var id) || id <= 0)
        {
            Console.Error.WriteLine("Invalid input.");
            return;
        }
        
        var contact =  await _contactRepository.GetByIdAsync(id);
        if (contact is null)
        {
            Console.WriteLine($"Contact not found with id [{id}].");
            return;
        }

        Console.WriteLine("NOTE: Leave field blank to keep current value");

        var newName = ReadInput($"Name: {contact.Name}", allowEmpty: true);
        if (!String.IsNullOrEmpty(newName)) contact.Name = newName;
        
        var newEmail = ReadInput($"Email: {contact.Email}", isEmail: true, allowEmpty: true);
        if (!String.IsNullOrEmpty(newEmail)) contact.Email = newEmail;
        
        var newPhone = ReadInput($"Phone: {contact.Phone}", isPhoneNumber: true, allowEmpty: true);
        if (!String.IsNullOrEmpty(newPhone)) contact.Phone = newPhone;
        
        await _contactRepository.UpdateContactAsync(contact);
        Console.WriteLine("Contact updated.");

    }

    private async Task HandleDeleteContact()
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Delete Contact");
        Console.WriteLine(new string('-', 50));
        
        Console.WriteLine("Enter contact id:");
        var input =  Console.ReadLine()?.Trim() ?? "";

        if (!Int32.TryParse(input, out var id) || id <= 0)
        {
            Console.Error.WriteLine("Invalid input.");
        }

        try
        {
            await _contactRepository.DeleteContactAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"No contact exists with id [{id}]");
            return;
        }
        
        Console.WriteLine("Contact deleted.");
    }

    private async Task HandleViewContact()
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("View Contact");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("Enter contact id:");
        var input =  Console.ReadLine()?.Trim() ?? "";

        if (!Int32.TryParse(input, out var id) || id <= 0)
        {
            Console.Error.WriteLine("Invalid input.");
            return;
        }

        var contact = await _contactRepository.GetByIdAsync(id);

        if (contact is null)
        {
            Console.WriteLine($"Contact not found with id [{id}].");
            return;
        }

        Console.WriteLine(contact);
    }
    
    private async Task HandleListContacts()
    {
        var contacts = (await _contactRepository.GetContactsAsync()).ToList();

        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found.");
            return;
        }
        
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Contact list:");
        Console.WriteLine(new string('-', 50));


        foreach (var contact in contacts)
        {
            Console.WriteLine(contact);
        }
    }
    
    private async Task HandleSearch()
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Search");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("Enter search keyword: ");
        var keyword = Console.ReadLine()?.Trim();

        var contacts = (await _contactRepository.GetByKeywordAsync(keyword)).ToList();

        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found for specified keyword...");
            return;
        }
        
        foreach (var contact in contacts)
        {
            Console.WriteLine(contact);
        }
        
    }
    
    private async Task HandleFilter()
    {   
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("Filter Contacts");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("NOTE: Leave field blank to ignore it.");

        Console.WriteLine("Name: ");
        var name = Console.ReadLine()?.Trim();
        
        Console.WriteLine("Email: ");
        var email= Console.ReadLine()?.Trim();

        Console.WriteLine("Phone: ");
        var phone = Console.ReadLine()?.Trim();

        Console.WriteLine("Created after (yyyy-MM-dd)): ");
        DateTime? createdAfter = DateTime.TryParse(Console.ReadLine()?.Trim(), out var afterParsed) ? afterParsed : null;

        Console.WriteLine("Created before (yyyy-MM-dd)): ");
        DateTime? createdBefore = DateTime.TryParse(Console.ReadLine()?.Trim(), out var beforeParsed) ? beforeParsed : null;

        var contacts = (await _contactRepository.GetByFilterAsync(name, email, phone, createdAfter, createdBefore)).ToList();

        if (!contacts.Any())
        {
            Console.WriteLine("No contacts found with specified filters.");
            return;
        }

        foreach (var contact in contacts)
        {
            Console.WriteLine(contact);
        }
    }
    
    private async Task HandleSave()
    {
        await _contactRepository.SaveAsync();
        Console.WriteLine("Save complete.");
    }

    private async Task HandleExit()
    {
        Console.WriteLine("Exiting...");
        _running = false;
    }

    private async Task HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                await HandleAddContact();
                break;
            case "2":
                await HandleEditContact();
                break;
            case "3":
                await HandleDeleteContact();
                break;
            case "4":
                await HandleViewContact();
                break;
            case "5":
                await HandleListContacts();
                break;
            case "6":
                await HandleSearch();
                break;
            case "7":
                await HandleFilter();
                break;
            case "8":
                await HandleSave();
                break;
            case "9":
                await HandleExit();
                // TODO: prompt to save before exit
                break;
            default:
                Console.Error.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                break;
        }
    }
    
    private string ReadInput(string prompt, bool isEmail = false, bool isPhoneNumber = false, bool allowEmpty = false)
    {
        Console.WriteLine(prompt);
    
        var input = Console.ReadLine()?.Trim() ?? "";
        
        if (string.IsNullOrEmpty(input))
        {
            if (allowEmpty)
            {
                return input;
            }
            
            Console.Error.WriteLine("Invalid input. Cannot be empty.");
            return ReadInput(prompt, isEmail, isPhoneNumber);
        }

        if (isEmail && !input.Contains("@"))
        {
            // minimal validation for development purposes
            Console.Error.WriteLine("Invalid email format..");
            return ReadInput(prompt, isEmail, isPhoneNumber);
        }

        if (isPhoneNumber)
        {
            // minimal validation for development purposes
            var digitsOnly = new string(input.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length < 7)
            {
                Console.Error.WriteLine("Invalid phone number. Must contain at least 7 digits.");
                return ReadInput(prompt, isEmail, isPhoneNumber);
            }
        }
    
        return input;
    }

    private void DisplayMenu()
    {
        // header
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("CONTACT MANAGEMENT");
        Console.WriteLine(new string('-', 50));
        
        // options
        Console.WriteLine("1) Add Contact");
        Console.WriteLine("2) Edit Contact");
        Console.WriteLine("3) Delete Contact");
        Console.WriteLine("4) View Contact");
        Console.WriteLine("5) List Contacts");
        Console.WriteLine("6) Search");
        Console.WriteLine("7) Filter");
        Console.WriteLine("8) Save");
        Console.WriteLine("9) Exit");
        
        // footer
        Console.WriteLine(new string('-', 50));
    }
}