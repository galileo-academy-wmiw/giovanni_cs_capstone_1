using BookstoreApp.Products;
using Microsoft.EntityFrameworkCore;
namespace BookstoreApp.Data;

public class BookstoreDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Order> Orders => Set<Order>(); 
    public DbSet<Customer> Customers => Set<Customer>(); // Customer accounts
    
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options) { } // Needed for BookstoreDbContextFactory
    public BookstoreDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // Determines where the db file gets created if none exists
    {
        var projectDir = Directory.GetCurrentDirectory(); 
        var dbPath = Path.Combine(projectDir, "bookstore.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Book inheritance mapping (Book, EBook, AudioBook) -> one table with discriminator
        modelBuilder.Entity<Book>()
            .HasDiscriminator<string>("ProductType")
            .HasValue<Book>("Book")
            .HasValue<EBook>("EBook")
            .HasValue<AudioBook>("Audiobook");
        
        // Book key mapping
        modelBuilder.Entity<Book>().HasKey(b => b.ProductId);

        // Price columns
        modelBuilder.Entity<Book>()
            .Property(b => b.PriceCents)
            .HasColumnType("INTEGER");

        // Order total
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalCents)
            .HasColumnType("INTEGER");

        // Order line item prices
        modelBuilder.Entity<OrderLine>()
            .Property(ol => ol.UnitPriceCents)
            .HasColumnType("INTEGER");

        // Order -> Lines relationship with cascade delete (deleting an order deletes its lines)
        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.Order)
            .WithMany(o => o.Lines)
            .HasForeignKey(ol => ol.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Customer entity configuration
        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .IsRequired();

        // Address is required for customers to ensure we have contact information and to distinguish customers with similar names
        modelBuilder.Entity<Customer>()
            .Property(c => c.Address)
            .IsRequired();

        // Order -> Customer relationship with restrict delete (cannot delete a customer if they have orders)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}

// <summary>
// BookstoreDbContext is the database context class for the bookstore application
// It derives from DbContext and provides access to the database sets for books, orders, order lines, and customers
// This class is responsible for configuring the database connection and defining the relationships between entities 
// </summary>