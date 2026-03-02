using BookstoreApp.Data;

namespace BookstoreApp
{
    public partial class UserInterface // Partial as the UserInterface is still one large class but uses file-level separation for ease of modification and readability
    {
        private void AdminViewOrdersFlow() // Function to display all orders
        {
            var type = PromptTypeFilter();
            var (sortByTotal, ascending) = PromptPriceSort("total price");

            var all = orders.QueryAllOrders(type, sortByTotal, ascending);

            if (all.Count == 0)
            {
                Console.WriteLine("No orders found (matching filter).");
                return;
            }

            Console.WriteLine("== Orders ==");

            foreach (var o in all)
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
                    var lineTotal = line.UnitPriceCents * line.Quantity;
                    var typePart = string.IsNullOrWhiteSpace(line.ProductTypeSnapshot) ? "" : $" | {line.ProductTypeSnapshot}";
                    Console.WriteLine($"  - {line.ProductNameSnapshot}{typePart} | {line.Quantity} x {FormatMoney(line.UnitPriceCents)} = {FormatMoney(lineTotal)}");
                }
            }
        }

        private void AdminCompleteOrderFlow() // Function to mark an order as completed
        {
            int id = PromptInt("Enter order id to mark completed: ", min: 1);

            var ok = orders.MarkCompleted(id);
            Console.WriteLine(ok ? "Order marked as completed." : "Order not found.");
        }

        private void AdminRejectOrderFlow() // Function to mark an order as rejected
        {
            int id = PromptInt("Enter order id to reject: ", min: 1);
            string reason = Prompt("Reason (optional): ");

            var ok = orders.Reject(id, reason);
            Console.WriteLine(ok ? "Order rejected." : "Order not found (or cannot reject).");
        }
    }
}

// <summary>
// Handles the admin order management.
// </summary>