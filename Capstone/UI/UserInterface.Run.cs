namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        public void Run() // Starts the user interface.
        {
            while (true)
            {
                Console.WriteLine("Login:");
                Console.WriteLine("  1) Administrator");
                Console.WriteLine("  2) Customer - Create account");
                Console.WriteLine("  3) Customer - Login with ID");
                Console.Write("Choose an option: ");

                var choice = (Console.ReadLine() ?? "").Trim();

                if (choice == "1") // Administrator login
                {
                    string password = Prompt("Enter admin password: ");
                    if (!auth.LoginAdmin(password))
                    {
                        Console.WriteLine("Invalid password.\n");
                        continue;
                    }

                    Console.WriteLine("Logged in as administrator.\n");
                    break;
                }

                if (choice == "2") // Customer account creation
                {
                    string name = Prompt("Name: ");
                    string address = Prompt("Address: ");

                    int newId = customerRepo.Create(name, address);
                    auth.LoginCustomer(newId);

                    Console.WriteLine($"\nAccount created! Your Customer ID is: {newId}");
                    Console.WriteLine("Use this ID to log in next time.\n");
                    break;
                }

                if (choice == "3") // Customer login
                {
                    int id = PromptInt("Enter Customer ID: ", min: 1);

                    var customer = customerRepo.GetById(id);
                    if (customer is null)
                    {
                        Console.WriteLine("No customer found with that ID.\n");
                        continue;
                    }

                    auth.LoginCustomer(id);
                    Console.WriteLine($"Welcome back, {customer.Name}!\n");
                    break;
                }

                Console.WriteLine("Invalid choice, try again.\n");
            }

            while (true) // Main application loop
            {
                DisplayMenu();
                var choice = (Console.ReadLine() ?? "").Trim();

                var action = VisibleActions().FirstOrDefault(a => a.Key == choice);
                if (action == null)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
                }

                action.Execute();
            }
        } 
    }
} 

// <summary>
// Provides the main entry point for the UI.
// </summary>
