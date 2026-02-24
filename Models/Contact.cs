using System.Text.Json.Serialization;

namespace MicrosoftContactManagement.Models;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Contact(string name, string phone, string email)
    {
        Name = name;
        Phone = phone;
        Email = email;
    }

    [JsonConstructor]
    public Contact(int id, string name, string phone, string email, DateTime creationDate) : this(name, phone, email)
    {
        Id = id;
        CreationDate = creationDate;
    }

    public override string ToString()
    {
        return
            $"[{Id}]: {nameof(Name)}: {Name}, {nameof(Phone)}: {Phone}, {nameof(Email)}: {Email}, {nameof(CreationDate)}: {CreationDate}";
    }
}