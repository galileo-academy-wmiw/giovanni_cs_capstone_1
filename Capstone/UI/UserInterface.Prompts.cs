namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private void ExitFlow()
        {
            auth.Logout();
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
        
        private static string FormatMoney(int cents)
        {
            return $"€{cents / 100m:0.00}";
        }
        
        private static string PromptText(string prompt) // Prompts for a text input, can not be empty.
        {
            while (true)
            {
                Console.Write(prompt);
                var s = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(s)) return s;
                Console.WriteLine("Value cannot be empty.");
            }
        }

        private static int PromptInt(string prompt, int min) // Prompts for an integer input, must be >= min.
        {
            while (true)
            {
                Console.Write(prompt);
                var raw = (Console.ReadLine() ?? "").Trim();
                if (int.TryParse(raw, out var value) && value >= min) return value;
                Console.WriteLine($"Please enter a valid number (>= {min}).");
            }
        }
        private static int PromptMoneyCents(string prompt) // Prompts for an integer input
        {
            while (true)
            {
                Console.Write(prompt);
                var raw = (Console.ReadLine() ?? "").Trim();

                // allow input like 12.99 or 12,99 depending on locale
                raw = raw.Replace(',', '.');

                if (decimal.TryParse(raw, out var euros) && euros >= 0)
                    return (int)Math.Round(euros * 100m, MidpointRounding.AwayFromZero);

                Console.WriteLine("Please enter a valid amount.");
            }
        }
        
        private string Prompt(string prompt) // Prompts for a string input.
        {
            Console.Write($"{prompt} ");
            return (Console.ReadLine() ?? "").Trim();
        }
        
        private static string PromptTypeFilter() // Filters by type
        {
            Console.WriteLine("Filter by type:");
            Console.WriteLine("  1) All");
            Console.WriteLine("  2) Book");
            Console.WriteLine("  3) EBook");
            Console.WriteLine("  4) Audiobook");
            Console.Write("Choice: ");

            return (Console.ReadLine() ?? "").Trim() switch
            {
                "2" => "Book",
                "3" => "EBook",
                "4" => "Audiobook",
                _ => "All"
            };
        }

        private static (bool sort, bool ascending) PromptPriceSort(string label) // Sorts by price
        {
            Console.WriteLine($"Sort by {label}?");
            Console.WriteLine("  1) Default");
            Console.WriteLine("  2) Ascending");
            Console.WriteLine("  3) Descending");
            Console.Write("Choice: ");

            return (Console.ReadLine() ?? "").Trim() switch
            {
                "2" => (true, true),
                "3" => (true, false),
                _ => (false, true)
            };
        }
        
    }
}

// <summary>
// Provides methods for user input prompts.
// </summary>