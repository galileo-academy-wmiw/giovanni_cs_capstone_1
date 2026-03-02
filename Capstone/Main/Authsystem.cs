namespace BookstoreApp.Main;

public class AuthSystem
{
    public int? CurrentUserId { get; private set; }
    
    public bool IsAdminLoggedIn { get; private set; }

    public bool IsAdmin() => IsAdminLoggedIn;

    public bool LoginAdmin(string password)
    {
        if (password == "admin")
        {
            IsAdminLoggedIn = true;
            CurrentUserId = null;
            return true;
        }
        return false;
    }

    public bool LoginCustomer(int customerId) 
    {
        if (customerId <= 0) return false;

        CurrentUserId = customerId;
        IsAdminLoggedIn = false;
        return true;
    }

    public void Logout()
    {
        CurrentUserId = null;
        IsAdminLoggedIn = false;
    }
}

// <summary>
// AuthSystem class handles user authentication and authorization
// It manages the login state of both admin and customer users
// A real implementation would involve more complex logic and security measures, but for this application, it provides a simple framework for user management
// </summary>