using BookstoreApp.Cart_management;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Data;

public class EfOrderRepository : IOrderRepository // implements the IOrderRepository interface
{
    private readonly BookstoreDbContext _db;

    public EfOrderRepository(BookstoreDbContext db) => _db = db;

    public int CreateOrderFromCart(int? customerId, ShoppingCart cart) // creates a new order from the shopping cart
    {
        if (customerId <= 0)
            throw new ArgumentException("Invalid customerId.", nameof(customerId));

        if (cart.IsEmpty())
            throw new InvalidOperationException("Cannot create an order from an empty cart.");

        var order = new Order
        {
            CustomerId = customerId,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            Status = OrderStatus.Open,
            TotalCents = cart.Total(),
            Lines = cart.Items.Select(ci => new OrderLine
            {
                ProductId = ci.Product.ProductId,
                Quantity = ci.Quantity,
                ProductNameSnapshot = ci.Product.Name,
                ProductTypeSnapshot = ci.Product.Type,
                UnitPriceCents = ci.Product.PriceCents
            }).ToList()
        };

        _db.Orders.Add(order);
        _db.SaveChanges();
        return order.OrderId;
    }

    public IReadOnlyList<Order> GetAll(bool includeLines = true) // retrieves all orders
    {
        var q = _db.Orders.AsNoTracking().OrderByDescending(o => o.CreatedAt);
        return includeLines ? q.Include(o => o.Lines).ToList() : q.ToList();
    }

    public IReadOnlyList<Order> GetByCustomer(int? customerId, bool includeLines = true) // retrieves all orders for a specific customer
    {
        var q = _db.Orders.AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt);

        return includeLines ? q.Include(o => o.Lines).ToList() : q.ToList();
    }

    public Order? GetById(int orderId, bool includeLines = true) // retrieves an order by its ID
    {
        var q = _db.Orders.AsNoTracking().Where(o => o.OrderId == orderId);
        return includeLines ? q.Include(o => o.Lines).FirstOrDefault() : q.FirstOrDefault();
    }

    public bool MarkCompleted(int orderId) // marks an order as completed
    {
        var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order is null) return false;

        if (order.Status == OrderStatus.Rejected) return false; // Disallow completing rejected
        if (order.Status == OrderStatus.Completed) return true;

        order.Status = OrderStatus.Completed;
        order.CompletedAt = DateTime.UtcNow;
        _db.SaveChanges();
        return true;
    }

    public bool Reject(int orderId, string reason) // rejects an order with the given ID and reason
    {
        var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order is null) return false;

        if (order.Status == OrderStatus.Completed) return false; // Disallow rejecting completed
        if (order.Status == OrderStatus.Rejected) return true;

        order.Status = OrderStatus.Rejected;
        order.RejectedAt = DateTime.UtcNow;
        order.RejectionReason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();

        _db.SaveChanges();
        return true;
    }
    
    public IReadOnlyList<Order> QueryAllOrders(string? typeFilter, bool sortByTotal, bool ascending) //sorting queries
{
    var q = _db.Orders.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(typeFilter) && typeFilter != "All")
        q = q.Where(o => o.Lines.Any(l => l.ProductTypeSnapshot == typeFilter));

    q = q.Include(o => o.Lines);

    if (sortByTotal)
        q = ascending ? q.OrderBy(o => o.TotalCents) : q.OrderByDescending(o => o.TotalCents);
    else
        q = q.OrderByDescending(o => o.CreatedAt);

    return q.ToList();
}

public IReadOnlyList<Order> QueryCustomerOrders(int? customerId, string? typeFilter, bool sortByTotal, bool ascending)
{
    var q = _db.Orders.AsNoTracking()
        .Where(o => o.CustomerId == customerId);

    if (!string.IsNullOrWhiteSpace(typeFilter) && typeFilter != "All")
        q = q.Where(o => o.Lines.Any(l => l.ProductTypeSnapshot == typeFilter));

    q = q.Include(o => o.Lines);

    if (sortByTotal)
        q = ascending ? q.OrderBy(o => o.TotalCents) : q.OrderByDescending(o => o.TotalCents);
    else
        q = q.OrderByDescending(o => o.CreatedAt);

    return q.ToList();
}
    
}

// <summary>
// EfOrderRepository is a class that implements the IOrderRepository interface
// It uses Entity Framework Core to interact with the database
// This class provides methods to add, remove, and retrieve orders from the database
// </summary>