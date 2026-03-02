namespace BookstoreApp.Main
{
    public class User
    {
        public string Username { get; }
        
        public string Password { get; }
        
        public bool IsAdmin { get; }

        public User(string username, string password, bool isAdmin)
        {
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }

        public bool Validate(string username, string password)
        {
            return Username.Equals(username, System.StringComparison.OrdinalIgnoreCase) && Password == password;
        }
    }
}


// <summary>
// Represents a user in the system.
// </summary>