using BookstoreApp.Data;

namespace BookstoreApp
{
    public partial class UserInterface // partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private void ViewCatalogFlow()
        {
            var type = PromptTypeFilter();
            var (sortByPrice, ascending) = PromptPriceSort("price");

            var all = catalog.Query(type, sortByPrice, ascending);

            if (all.Count == 0)
            {
                Console.WriteLine("No products match your filter.");
                return;
            }

            Console.WriteLine("== Catalog ==");
            foreach (var p in all)
                Console.WriteLine($"{p.Name} | {p.Type} | {FormatMoney(p.PriceCents)}");            
        }

        private void AddToCartByNameFlow() // function to add a product to the cart by name, customers do not know the SKU or what that is most of the time
        {
            var product = SelectCatalogProductByName("Enter product title/name to add: ");
            if (product == null) return;

            var qty = PromptInt("Quantity: ", min: 1);

            cart.Add(product, qty);
            Console.WriteLine($"Added to cart: {product.Name} | {product.Type} (x{qty})");
        }

        private void RemoveFromCartByNameFlow() // function to remove a product from the cart by name
        {
            if (cart.IsEmpty())
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            var item = SelectCartItemByName("Enter product title/name to remove: ");
            if (item == null) return;

            Console.Write("Quantity to remove (default 1): ");
            var raw = (Console.ReadLine() ?? "").Trim();
            int qty = 1;

            if (!string.IsNullOrWhiteSpace(raw))
            {
                if (!int.TryParse(raw, out qty) || qty <= 0) // Validate quantity input
                {
                    Console.WriteLine("Invalid quantity.");
                    return;
                }
            }

            var ok = cart.Remove(item.Product.ProductId, qty);
            Console.WriteLine(ok ? "Cart updated." : "Failed to update cart."); // Update cart item quantity, fail when item is not found or user inputs 0
        }

        private void ViewCartFlow() // function to display the shopping cart
        {
            if (cart.IsEmpty())
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("== Your Cart ==");
            foreach (var item in cart.Items)
            {
                var lineTotal = item.Product.PriceCents * item.Quantity;
                Console.WriteLine($"{item.Product.Name} | {item.Product.Type} | {item.Quantity} x {FormatMoney(item.Product.PriceCents)} = {FormatMoney(lineTotal)}"); // Display each item in the cart with its quantity, unit price, and line total
            }

            Console.WriteLine($"Total: {FormatMoney(cart.Total())}");
        }

        private void PlaceOrderFlow() // function to place an order
        {
            if (cart.IsEmpty())
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("== Order Summary ==");
            foreach (var item in cart.Items)
            {
                var lineTotal = item.Product.PriceCents * item.Quantity;
                Console.WriteLine($"{item.Product.Name} | {item.Product.Type} | {item.Quantity} x {FormatMoney(item.Product.PriceCents)} = {FormatMoney(lineTotal)}");
            }

            Console.WriteLine($"Order total: {FormatMoney(cart.Total())}");

            Console.Write("Place order? (y/n): ");
            var confirm = (Console.ReadLine() ?? "").Trim().ToLowerInvariant(); // Simple confirmation prompt, ToLowerInvariant for case-insensitivity
            if (confirm != "y")
            {
                Console.WriteLine("Order cancelled.");
                return;
            }

            // Cart persistence via EF
            var orderId = orders.CreateOrderFromCart(auth.CurrentUserId, cart);
            cart.Clear();
            Console.WriteLine($"Order placed! Your order number is #{orderId}.");
        }

        private void ViewMyOrdersFlow() // Function to view the logged-in customer's orders
        {
            var customerId = auth.CurrentUserId;

            var type = PromptTypeFilter();
            var (sortByTotal, ascending) = PromptPriceSort("total price");

            var myOrders = orders.QueryCustomerOrders(customerId, type, sortByTotal, ascending);

            if (myOrders.Count == 0)
            {
                Console.WriteLine("No orders match your filter.");
                return;
            }

            Console.WriteLine("== My Orders ==");

            foreach (var o in myOrders)
            {
                string statusText = o.Status switch
                {
                    OrderStatus.Open => "OPEN",
                    OrderStatus.Completed => $"COMPLETED ({o.CompletedAt:u})",
                    OrderStatus.Rejected => $"REJECTED ({o.RejectedAt:u})",
                    _ => o.Status.ToString().ToUpperInvariant()
                };

                Console.WriteLine($"Order #{o.OrderId} | {o.CreatedAt:u} | {statusText} | Total: {FormatMoney(o.TotalCents)}");

                if (o.Status == OrderStatus.Rejected && !string.IsNullOrWhiteSpace(o.RejectionReason))
                    Console.WriteLine($"  Reason: {o.RejectionReason}");

                foreach (var line in o.Lines)
                {
                    var lineTotal = line.UnitPriceCents  * line.Quantity;
                    var typePart = string.IsNullOrWhiteSpace(line.ProductTypeSnapshot) ? "" : $" | {line.ProductTypeSnapshot}";
                    Console.WriteLine($"  - {line.ProductNameSnapshot}{typePart} | {line.Quantity} x {FormatMoney(line.UnitPriceCents)} = {FormatMoney(lineTotal)}");
                }
            }
        }
        
    }
}

// <summary>
// Handles the customer shopping.
// </summary>