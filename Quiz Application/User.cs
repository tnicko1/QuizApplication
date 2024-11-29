using System.Text.Json;

namespace Quiz_Application;

public class User
{
    private string Name { get; set; }
    private string? Email { get; set; }
    private string? Password { get; set; }
    private string? Role { get; set; }
    
    public static UserDatabase? UserDatabase { get; set; }

    private User(string name, string? email, string? password, string? role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public static User CreateUser(string name, string? email, string? password, string? role, UserDatabase? database = null)
    {
        var user = new User(name, email, password, role);
        database?.AddUser(user);
        return user;
    }
    
    public void Display()
    {
        Console.WriteLine($"Name: {Name}\nEmail: {Email}\nRole: {Role}\n");
    }
    
    public void ChangePassword(string? newPassword)
    {
        Password = newPassword;
        
    }
    
    public void ChangeRole(string? newRole)
    {
        Role = newRole;
    }
    
    public void ChangeEmail(string? newEmail)
    {
        Email = newEmail;
    }
    
    public void ChangeName(string newName)
    {
        Name = newName;
    }
    
    public string GetName()
    {
        return Name;
    }
    
    public string? GetEmail()
    {
        return Email;
    }
    
    public string? GetRole()
    {
        return Role;
    }
    
    public string? GetPassword()
    {
        return Password;
    }
}