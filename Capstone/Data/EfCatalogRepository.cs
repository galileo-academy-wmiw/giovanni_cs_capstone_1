using BookstoreApp.Interfaces;
using BookstoreApp.Products;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Data;

public class EfCatalogRepository : ICatalogRepository
{
    private readonly BookstoreDbContext _db; // read-only database context to ensure that the catalog is not modified directly through this repository
    public EfCatalogRepository(BookstoreDbContext db) => _db = db; // constructor

    public IProduct? GetById(string id)
    {
        if (!int.TryParse(id, out var parsedId))
            return null;

        return _db.Books
            .AsNoTracking()
            .FirstOrDefault(b => b.ProductId == parsedId);
    }
    
    public IReadOnlyList<IProduct> GetAll()
        => _db.Books.AsNoTracking().Cast<IProduct>().ToList(); // retrieves all books from the database and returns them as a list of IProduct, using AsNoTracking for better performance since the method is read-only
    
    public IProduct? GetById(int id)
        => _db.Books.AsNoTracking().FirstOrDefault(b => b.ProductId == id); // retrieves a book by its ID, using AsNoTracking for better performance since the method is read-only

    public bool Add(IProduct product) // adds a new product to the catalog
    {
        if (product is not Book book) return false;

        if (book.ProductId <= 0) return false;
        if (_db.Books.Any(b => b.ProductId == book.ProductId)) return false;

        _db.Books.Add(book);
        _db.SaveChanges();
        return true;
    }
    public bool RemoveById(int id) // removes a product from the catalog by ID 
    {
        var book = _db.Books.FirstOrDefault(b => b.ProductId == id); // FirstOrDefault retrieves the first matching book or null
        if (book is null) return false;

        _db.Books.Remove(book);
        _db.SaveChanges();
        return true;
    }

    public IReadOnlyList<IProduct> Query(string? typeFilter, bool sortByPrice, bool ascending)
    {
        var q = _db.Books.AsNoTracking().AsQueryable();
        
        // discriminator column is "ProductType"
        if (!string.IsNullOrWhiteSpace(typeFilter) && typeFilter != "All")
            q = q.Where(b => EF.Property<string>(b, "ProductType") == typeFilter);

        if (sortByPrice)
            q = ascending ? q.OrderBy(b => b.PriceCents) : q.OrderByDescending(b => b.PriceCents);

        return q.Cast<IProduct>().ToList();
    }
}

// <summary>
// EFCatalogRepository is a class that implements the ICatalogRepository interface
// It uses Entity Framework Core to interact with the database
// This class provides methods to add, remove, and retrieve products from the database
// </summary>