using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Common.Exceptions;

namespace Triumph.SMS.Remote.Core.ApplicationUsers;

public class ApplicationUser : Person
{
    // For EF
    private ApplicationUser() {}

    private ICollection<PrimaryPhone> _Contacts { get; set; }
    public IReadOnlyCollection<PrimaryPhone> Contacts => _Contacts.ToList().AsReadOnly();

    public string Username { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string HashedPassword { get; private set; } = string.Empty;
    public string? Roles { get; private set; } // e.g., "Admin,Accountact" = comma separated of the Role enum type

    public ApplicationUser Register (
        string firstName,
        string lastName,
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
        
        if (roles != null)
        {
            SetRoles(roles);
        }

        var newUser = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Email = email,
            HashedPassword = hashedPassword,
            _Contacts = phones?.ToList() ?? []
        };

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
        var roles = Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return roles.Contains(role.ToString());
    }

    public void SetRoles(IEnumerable<Role> roles)
    {
        foreach (var role in roles) 
        {
            if (HasRole(role)) {
                throw new BadValuesException($"User already has role: {role}");
            }
        }

        Roles = string.Join(',', roles.Select(r => r.ToString()));
        UpdatedAt = DateTime.UtcNow;
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

    private bool PasswordIsNotValid(string password)
    {
        // Example password validation: at least 8 characters, one uppercase, one lowercase
        if (password.Length < 8)
            return true;

        if (!password.Any(char.IsUpper))
            return true;

        if (!password.Any(char.IsLower))
            return true;

        return false;
    }
}
