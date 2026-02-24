using System.Reflection.Metadata;
using MicrosoftContactManagement.Repositories;

namespace MicrosoftContactManagement;

public class ContactManagementApplication
{
    private readonly IContactRepository _contactRepository;
    private bool _running = true;
    public ContactManagementApplication(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task RunAsync()
    {
        await _contactRepository.LoadAsync();

        while (_running)
        {
            DisplayMenu();
            var input = Console.ReadLine()?.Trim() ?? String.Empty;
            await HandleInput(input);
        }
    }

    private async Task HandleInput(string input)
    {
        switch (input)
        {
            case "1":
                // await HandleAddContact();
                break;
            case "2":
                // await HandleEditContact();
                break;
            case "3":
                // await HandleDeleteContact();
                break;
            case "4":
                // await HandleViewContact();
                break;
            case "5":
                // await HandleListContacts();
                break;
            case "6":
                // await HandleSearch();
                break;
            case "7":
                // await HandleFilter();
                break;
            case "8":
                // await HandleSave();
                break;
            case "9":
                // await HandleExit();
                break;
            default:
                Console.Error.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                await Task.Delay(1000);
                break;
        }
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