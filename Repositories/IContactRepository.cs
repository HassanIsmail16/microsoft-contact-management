using MicrosoftContactManagement.Models;

namespace MicrosoftContactManagement.Repositories;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task<int> InsertContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int id);
    Task SaveAsync();
    Task LoadAsync();
    Task<IEnumerable<Contact>> GetByKeywordAsync(string? keyword);
}