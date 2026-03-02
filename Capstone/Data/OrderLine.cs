namespace BookstoreApp.Data;

public class OrderLine
{
    public int OrderLineId { get; set; }
    
    public int OrderId { get; set; }
    
    public Order Order { get; set; } = null!;

    // Link to product
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }

    // Snapshots so history doesn’t change if catalog changes:
    public string ProductNameSnapshot { get; set; } = "";
    
    public string ProductTypeSnapshot { get; set; } = "";
    
    public int UnitPriceCents { get; set; }
}

// <summary>
// OrderLine class represents a line item in an order
// It contains properties for the line item's ID, order ID, product ID, quantity, and snapshots of product information
// This class is used to manage the details of each item in a customer's order
// </summary>