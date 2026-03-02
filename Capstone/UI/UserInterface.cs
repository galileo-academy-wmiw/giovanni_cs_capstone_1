using BookstoreApp.Cart_management;
using BookstoreApp.Data;
using BookstoreApp.Interfaces;
using BookstoreApp.Main;

namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private readonly AuthSystem auth = new AuthSystem();


        /// Storefront state
        private readonly ICatalogRepository catalog;
        private readonly ShoppingCart cart = new ShoppingCart();
        private readonly List<UserInterface.MenuAction> actions;
        private readonly IOrderRepository orders;
        private readonly EfCustomerRepository customerRepo;

        public UserInterface(ICatalogRepository catalogRepository, IOrderRepository orderRepo, EfCustomerRepository customerRepository)
        {
            catalog = catalogRepository;
            orders = orderRepo;
            customerRepo = customerRepository;

            actions = new List<UserInterface.MenuAction>
            {
                // Admin: inventory
                new UserInterface.MenuAction("1", "Add product to catalog",        () => auth.IsAdmin(), AdminAddProductFlow),
                new UserInterface.MenuAction("2", "Remove product from catalog",   () => auth.IsAdmin(), AdminRemoveProductFlow),
                new UserInterface.MenuAction("3", "View catalog",                 () => auth.IsAdmin(), ViewCatalogFlow),

                // Admin: customer order management
                new UserInterface.MenuAction("4", "View orders",                  () => auth.IsAdmin(), AdminViewOrdersFlow),
                new UserInterface.MenuAction("5", "Mark order as completed",      () => auth.IsAdmin(), AdminCompleteOrderFlow),
                new UserInterface.MenuAction("6", "Mark order as rejected",       () => auth.IsAdmin(), AdminRejectOrderFlow),

                // Customer: shopping
                new UserInterface.MenuAction("1", "Add item to cart (by title)",          () => !auth.IsAdmin(), AddToCartByNameFlow),
                new UserInterface.MenuAction("2", "Remove item from cart (by title)",     () => !auth.IsAdmin(), RemoveFromCartByNameFlow),
                new UserInterface.MenuAction("3", "View cart",                            () => !auth.IsAdmin(), ViewCartFlow),
                new UserInterface.MenuAction("4", "Place order",                          () => !auth.IsAdmin(), PlaceOrderFlow),
                new UserInterface.MenuAction("5", "View my orders",                       () => !auth.IsAdmin(), ViewMyOrdersFlow),
                new UserInterface.MenuAction("6", "View catalog",                         () => !auth.IsAdmin(), ViewCatalogFlow),

                // Exits the application
                new UserInterface.MenuAction("0", "Exit", () => true, ExitFlow),
            };
        }
    }
}

// <summary>
// Represents the user interface for the bookstore application, providing a menu-driven interface for users to interact with the system.
// </summary>