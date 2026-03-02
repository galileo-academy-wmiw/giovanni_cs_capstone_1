namespace BookstoreApp.Interfaces;

public interface IProduct
{
    int ProductId { get; }     // Stock Keeping Unit (or derived from ID)
    
    string Name { get; }    // Display name (or Title)
    
    int PriceCents { get; }
    
    string Type { get; }
    
}

// <summary>
// IProduct interface defines the properties and methods that a product must implement
// It serves as a contract for different types of products in the bookstore
// </summary>