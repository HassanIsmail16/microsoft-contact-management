using System.Text.Json.Serialization;

namespace MicrosoftContactManagement.Models;

public class Contact : IEquatable<Contact>
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

    public Contact(int id, string name, string phone, string email) : this(name, phone, email)
    {
        Id = id;
    }

    [JsonConstructor]
    public Contact(int id, string name, string phone, string email, DateTime creationDate) : this(id, name, phone, email)
    {
        CreationDate = creationDate;
    }
    
    public bool Equals(Contact? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name && Phone == other.Phone && Email == other.Email && CreationDate.Equals(other.CreationDate);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Contact)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Phone, Email, CreationDate);
    }

    public override string ToString()
    {
        return
            $"[{Id}]: {nameof(Name)}: {Name}, {nameof(Phone)}: {Phone}, {nameof(Email)}: {Email}, {nameof(CreationDate)}: {CreationDate}";
    }
}