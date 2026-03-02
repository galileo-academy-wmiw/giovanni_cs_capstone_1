using BookstoreApp.Cart_management;
using BookstoreApp.Interfaces;

namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private IProduct? SelectCatalogProductByName(string prompt) // Prompts for a product name and selects it from the catalog.
        {
            Console.Write(prompt);
            var query = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Please enter a name/title.");
                return null;
            }

            var matches = catalog.GetAll()
                .Where(p => !string.IsNullOrWhiteSpace(p.Name) &&
                            p.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) // Check if product name matches query
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("No products found with that name/title.");
                return null;
            }

            if (matches.Count == 1)
                return matches[0];

            Console.WriteLine("Multiple matches found:"); // if multiple matches are found, display them
            for (int i = 0; i < matches.Count; i++)
                Console.WriteLine($"{i + 1}. {matches[i].Name} | {matches[i].Type} | €{FormatMoney(matches[i].PriceCents)}");

            int pick = PromptInt("Choose a number: ", min: 1); // Prompt the user to choose a product by number
            if (pick > matches.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }

            return matches[pick - 1];
        }

        private CartItem? SelectCartItemByName(string prompt) // Prompts for a cart item name and selects it.
        {
            Console.Write(prompt);
            var query = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Please enter a name/title.");
                return null;
            }

            var matches = cart.Items
                .Where(i => !string.IsNullOrWhiteSpace(i.Product.Name) &&
                            i.Product.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) // Check if cart item name matches query
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("No cart items found with that name/title.");
                return null;
            }

            if (matches.Count == 1)
                return matches[0];

            Console.WriteLine("Multiple cart items match:");
            for (int i = 0; i < matches.Count; i++)
                Console.WriteLine($"{i + 1}. {matches[i].Product.Name} | {matches[i].Product.Type} | Qty: {matches[i].Quantity} | {FormatMoney(matches[i].Product.PriceCents)}");

            int pick = PromptInt("Choose a number: ", min: 1);
            if (pick > matches.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }

            return matches[pick - 1];
        }
    }
}   

// <summary>
// Provides selection helpers for the user interface.
// </summary>