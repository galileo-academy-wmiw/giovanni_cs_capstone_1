namespace BookstoreApp.Data;

public class Order
{
    public int OrderId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsCompleted { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Open;

    public DateTime? CompletedAt { get; set; }
    
    public DateTime? RejectedAt { get; set; }
    
    public string? RejectionReason { get; set; }

    public int TotalCents { get; set; }
    
    public List<OrderLine> Lines { get; set; } = new();

    public int? CustomerId { get; set; }
    
    public Customer? Customer { get; set; }



}

// <summary>
// Order class represents an order in the bookstore
// It contains properties for the order's ID, creation date, status, total amount, and a list of order lines
// This class is used to manage order information and track the customer's purchases
// </summary>