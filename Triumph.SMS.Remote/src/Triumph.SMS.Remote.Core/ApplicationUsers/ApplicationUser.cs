using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.Exceptions;

namespace Triumph.SMS.Remote.Core.ApplicationUsers;

public class ApplicationUser : Person
{
    // For EF
    private ApplicationUser() {}

    private ICollection<PrimaryPhone> _Contacts { get; set; } = [];
    public IReadOnlyCollection<PrimaryPhone> Contacts => _Contacts.ToList().AsReadOnly();

    public string Username { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string HashedPassword { get; private set; } = string.Empty;
    public string? Roles { get; private set; } // e.g., "Admin,Accountact" = comma separated of the Role enum type

    public static ApplicationUser Register (
        string firstName,
        string lastName,
        string otherNames,
        string username,
        string? email,
        string password,
        IEnumerable<Role>? roles,
        IEnumerable<PrimaryPhone>? phones)
    {
        if (PasswordIsNotValid(password))
        {
            throw new EntityValidationException("Password should be at least 8 characters long, should have at least one upper and lower case characters");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var newUser = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            OtherNames = otherNames,
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            _Contacts = phones?.ToList() ?? []
        };
        
        if (roles != null)
        {
            newUser.Roles = string.Join(',', roles.Select(r => r.ToString()));
        }

        return newUser;
    }

    public void AddContact(PrimaryPhone contact)
    {
        _Contacts.Add(contact);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveContact(PrimaryPhone contact) 
    {
        _Contacts.Remove(contact);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasRole(Role role)
    {
        if (string.IsNullOrEmpty(Roles))
        {
            return false;
        }
        
        var roles = Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return roles.Contains(role.ToString());
    }

    public void SetRoles(List<Role> roles)
    {
        if (roles == null || roles.Count == 0)
        {
            throw new EntityValidationException("Roles cannot be null");
        }
        
        foreach (var role in roles.Where(HasRole))
        {
            throw new BadValuesException($"User already has role: {role}");
        }

        Roles = string.Join(',', roles.Select(r => r.ToString()));
        UpdatedAt = DateTime.UtcNow;
    }
    
    public IEnumerable<Role> GetRoles() 
    {
        if (string.IsNullOrWhiteSpace(Roles))
        {
            return [];
        }

        var roles = Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return roles.Select(Enum.Parse<Role>);
    }

    public void ChangePassword(string newPassword)
    {
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
    }

    private static bool PasswordIsNotValid(string password)
    {
        // Example password validation: at least 8 characters, one uppercase, one lowercase
        if (password.Length < 8)
            return true;

        if (!password.Any(char.IsUpper))
            return true;

        return !password.Any(char.IsLower);
    }
}
