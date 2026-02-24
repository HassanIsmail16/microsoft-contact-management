using System.Text.Json;
using MicrosoftContactManagement.Models;

namespace MicrosoftContactManagement.Repositories;

public class JsonContactRepository(string filePath, JsonSerializerOptions jsonSerializerOptions)
    : IContactRepository
{
    private readonly string _filePath = filePath;
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

    private List<Contact> _contacts;
    private int _nextId = 1;
    
    public Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return Task.FromResult(_contacts.AsEnumerable());
    }

    public Task<Contact?> GetByIdAsync(int id)
    {
        return Task.FromResult(_contacts.FirstOrDefault(c => c.Id == id));
    }

    public Task<int> InsertContactAsync(Contact contact)
    {
        contact.Id = _nextId++;
        _contacts.Add(contact);
        return Task.FromResult(contact.Id);
    }

    public Task UpdateContactAsync(Contact contact)
    {
        var existingContact = _contacts.FirstOrDefault(c => c.Id == contact.Id);
        if (existingContact is null)
        {
            throw new InvalidOperationException($"Contact with id {contact.Id} not found");
        }
        
        existingContact.Name = contact.Name;
        existingContact.Email = contact.Email;
        existingContact.Phone = contact.Phone;
        
        return Task.CompletedTask;
    }

    public Task DeleteContactAsync(int id)
    {
        var existingContact = _contacts.FirstOrDefault(c => c.Id == id); 
        if (existingContact is null)
        { 
            throw new InvalidOperationException($"Contact with id [{id}] not found");
        }
        
        _contacts.Remove(existingContact);
        
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        var data = JsonSerializer.Serialize(_contacts, _jsonSerializerOptions);
        await File.WriteAllTextAsync(_filePath, data);
    }

    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            _contacts = new List<Contact>();
            _nextId = 1;
            return;
        }
        
        var data = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(data))
        {
            _contacts = new List<Contact>();
            _nextId = 1;
            return;
        }

        _contacts = JsonSerializer.Deserialize<List<Contact>>(data) ?? new List<Contact>();
        _nextId = _contacts.Any() ? _contacts.Max(c => c.Id) + 1 : 1;
    }
}