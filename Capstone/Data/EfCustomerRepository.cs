namespace BookstoreApp.Data;

public class EfCustomerRepository
{
    private readonly BookstoreDbContext _db; // read-only database context to ensure that the customer is not modified directly through this repository
    
    public EfCustomerRepository(BookstoreDbContext db) => _db = db; 

    public int Create(string name, string address) // creates a new customer
    {
        var c = new Customer { Name = name.Trim(), Address = address.Trim() };
        _db.Customers.Add(c);
        _db.SaveChanges();
        return c.Id;
    }

    public Customer? GetById(int id) // retrieves a customer by their ID
        => _db.Customers.Find(id);
}

// <summary>
// EfCustomerRepository is a class that implements the ICustomerRepository interface
// It uses Entity Framework Core to interact with the database
// This class provides methods to add, remove, and retrieve customer information from the database
// </summary>