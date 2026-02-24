using System.Text.Json;
using MicrosoftContactManagement.Models;
using MicrosoftContactManagement.Repositories;

namespace MicrosoftContactManagement;

class Program
{
    static async Task Main(string[] args)
    {
        JsonContactRepository repo = new JsonContactRepository(
            "contacts.json", 
            new JsonSerializerOptions{ WriteIndented = true }
        );
        ContactManagementApplication app = new ContactManagementApplication(repo);
        await app.RunAsync();
    }
}
