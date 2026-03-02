namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private sealed class MenuAction // Represents a single action in the menu, sealed to prevent inheritance, to ensure that all menu actions are defined in a single place and cannot be modified or extended externally.
        {
            public string Key { get; } // The unique key for the menu action
            public string Label { get; } // The display label for the menu item
            public Func<bool> IsVisible { get; } // Function to determine if the menu item is visible
            public Action Execute { get; } // The action to execute when the menu item is selected

            public MenuAction(string key, string label, Func<bool> isVisible, Action execute)  // Constructor for MenuAction
            {
                Key = key;
                Label = label;
                IsVisible = isVisible;
                Execute = execute;
            }
        }

        private IEnumerable<MenuAction> VisibleActions() => actions.Where(a => a.IsVisible()); // Get all visible menu actions

        private void DisplayMenu() // Display the menu
        {
            Console.WriteLine();
            Console.WriteLine(auth.IsAdmin() ? "== Admin: Inventory Management ==" : "== Customer: Storefront ==");

            foreach (var action in VisibleActions())
                Console.WriteLine($"{action.Key}. {action.Label}");

            Console.Write("Select option: ");
        }
    }
}

    // <summary>
    // Displays the main menu and handles user input.
    // </summary>