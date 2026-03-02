using BookstoreApp.Products;

namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private void AdminAddProductFlow()
        {
            Console.WriteLine("Product Types:");
            Console.WriteLine("1. Book");
            Console.WriteLine("2. eBook");
            Console.WriteLine("3. Audiobook");
            Console.Write("Choose type: ");
            var typeChoice = (Console.ReadLine() ?? "").Trim();

            Book product;
            if (typeChoice == "1") product = new Book();
            else if (typeChoice == "2") product = new EBook();
            else if (typeChoice == "3") product = new AudioBook();
            else
            {
                Console.WriteLine("Invalid type.");
                return;
            }

            FillCommonBookFields(product);

            product.PriceCents = PromptMoneyCents("Enter price: "); // Price must exist on Book, this method will let the user set it

            var ok = catalog.Add(product);
            Console.WriteLine(ok
                ? $"Added to catalog: {product.Name} (ID: {product.ProductId})"
                : "Failed to add product (ID already exists or invalid)."); // ID is unique and must not exist in the catalog
        }

        private void AdminRemoveProductFlow()
        {
            Console.Write("Enter ID to remove (admin/internal): "); // ID is the shortest input and less error-prone than title for removal, and is the unique identifier for products in the catalog
            var id = PromptInt("Enter ID to remove: ", min: 1);
            var ok = catalog.RemoveById(id);
            Console.WriteLine(ok ? "Removed from catalog." : "No product found with that SKU.");
        }

        private void FillCommonBookFields(Book book)
        {
            book.Title = PromptText("Enter title: ");
            book.PublicationYear = PromptInt("Enter publication year: ", min: 0);
            book.ProductId = PromptInt("Enter ID: ", min: 0);

            book.Genre = PromptText("Enter genre: ");
        }
    }
}

// <summary>
// Handles the admin catalog management.
// </summary>