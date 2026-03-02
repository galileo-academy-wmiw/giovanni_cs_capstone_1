using BookstoreApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApp.Main
{
    static class Program
    {
        static void Main()
        {
            using var db = new BookstoreDbContext();
            db.Database.Migrate();              // <-- required
            Console.WriteLine(db.Database.GetDbConnection().DataSource); // temp debug
            Console.WriteLine($"DB PATH USED: {db.Database.GetDbConnection().DataSource}");
            Console.WriteLine($"MIGRATIONS: {string.Join(", ", db.Database.GetMigrations())}");
            Console.WriteLine($"APPLIED:    {string.Join(", ", db.Database.GetAppliedMigrations())}");

            var catalogRepo = new EfCatalogRepository(db);
            var orderRepo = new EfOrderRepository(db);
            var customerRepo = new EfCustomerRepository(db);

            var ui = new UserInterface(catalogRepo, orderRepo, customerRepo);
            ui.Run();
        }
    }
}

// <summary>
// Program class serves as the entry point for the bookstore application
// It initializes the database context and sets up the user interface
// </summary>