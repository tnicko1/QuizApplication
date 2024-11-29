using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Quiz_Application
{
    public class UserDatabase
    {
        private List<User> _users;
        private readonly string _filePath;

        public UserDatabase()
        {
            _filePath = Path.Combine("..","..","..", "users.json");
            _users = new List<User>();
            LoadUsersFromJson();
        }

        private void LoadUsersFromJson()
        {
            try 
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var usersDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
                    
                    if (usersDict != null)
                    {
                        foreach (var (name, userInfo) in usersDict)
                        {
                            if (userInfo.TryGetValue("Email", out var email) &&
                                userInfo.TryGetValue("Password", out var password) &&
                                userInfo.TryGetValue("Role", out var role))
                            {
                                // Check if a user with this email already exists before adding
                                if (!_users.Any(u => u.GetEmail() == email))
                                {
                                    var newUser = User.CreateUser(name, email, password, role);
                                    _users.Add(newUser);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            if (_users.Any(u => u.GetEmail() == user.GetEmail()))
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            _users.Add(user);
            SaveUsersToJson();
        }

        private void SaveUsersToJson()
        {
            try 
            {
                var usersDict = _users.ToDictionary(
                    u => u.GetName(), 
                    u => new Dictionary<string, string?>
                    {
                        { "Email", u.GetEmail() },
                        { "Password", u.GetPassword() },
                        { "Role", u.GetRole() }
                    }
                );

                var jsonOptions = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };

                var json = JsonSerializer.Serialize(usersDict, jsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        public IReadOnlyList<User> GetUsers() => _users.AsReadOnly();

        public void RemoveUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            if (_users.Remove(user))
            {
                SaveUsersToJson();
            }
        }

        public void ChangeRole()
        {
            Console.WriteLine("Enter email of user to make admin:");
            var email = Console.ReadLine();
            var subjectUser = FindUserByEmail(email);
            if (subjectUser != null)
            {
                subjectUser.ChangeRole("admin");
                Console.WriteLine("User is now an admin");
            }
            else
            {
                Console.WriteLine("User not found");
            }
            
            SaveUsersToJson();
        }

        public User FindUserByEmail(string? email)
        {
            return _users.FirstOrDefault(u => u.GetEmail() == email);
        }
        
        public User FindUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.GetName() == name);
        }
        
        public User FindUserByRole(string role)
        {
            return _users.FirstOrDefault(u => u.GetRole() == role);
        }
    }
}