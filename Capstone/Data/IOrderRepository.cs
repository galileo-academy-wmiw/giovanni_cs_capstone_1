using BookstoreApp.Cart_management;

namespace BookstoreApp.Data;

public interface IOrderRepository // Defines the contract for order-related data access
{
    int CreateOrderFromCart(int? customerId, ShoppingCart cart); // Creates a new order from the shopping cart
    
    IReadOnlyList<Order> GetAll(bool includeLines = true); // Retrieves all orders
    
    IReadOnlyList<Order> GetByCustomer(int? customerId, bool includeLines = true); // Retrieves all orders for a specific customer
    
    Order? GetById(int orderId, bool includeLines = true); // Retrieves an order by its ID
    
    bool MarkCompleted(int orderId); // Marks an order as completed
    
    bool Reject(int id, string reason); // Rejects an order with the given ID and reason
    
    IReadOnlyList<Order> QueryAllOrders(string? typeFilter, bool sortByTotal, bool ascending); // Queries for sorting and filtering
    
    IReadOnlyList<Order> QueryCustomerOrders(int? customerId, string? typeFilter, bool sortByTotal, bool ascending);
    
}

// <summary>
// Defines the contract for order-related data access
// </summary>