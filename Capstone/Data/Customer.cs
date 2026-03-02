namespace BookstoreApp.Data;

public class Customer
{
    public int Id { get; set; } // auto-generated when a new customer is added
    
    public string Name { get; set; } = "";
    
    public string Address { get; set; } = "";

    public List<Order> Orders { get; set; } = new(); // customer's orders
}

// <summary>
// customer class represents a customer in the bookstore
// it contains properties for the customer's ID, name, address, and a list of orders
// this class is used to manage customer information and their orders and track their order history
// </summary>