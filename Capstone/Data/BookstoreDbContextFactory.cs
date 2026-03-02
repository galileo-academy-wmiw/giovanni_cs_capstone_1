using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookstoreApp.Data;

public class BookstoreDbContextFactory : IDesignTimeDbContextFactory<BookstoreDbContext>
{
    public BookstoreDbContext CreateDbContext(string[] args)
    {
        var projectDir = Directory.GetCurrentDirectory();
        var dbPath = Path.Combine(projectDir, "bookstore.db");

        var options = new DbContextOptionsBuilder<BookstoreDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        return new BookstoreDbContext(options);
    }
}

// <summary>
// BookstoreDbContextFactory is a factory class that implements IDesignTimeDbContextFactory<BookstoreDbContext>, which is used by Entity Framework Core tools at design time to create instances of the BookstoreDb
// Context when running commands like migrations or updating the database. It provides a way to configure the DbContext with the correct options, such as the connection string, without relying on the application's runtime configuration. This is especially useful for ensuring that EF Core can access the database context during design time operations, even if the application's normal configuration is not available or is different from what is needed for design time tasks.
// </summary>