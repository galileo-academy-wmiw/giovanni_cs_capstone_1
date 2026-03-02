using BookstoreApp.Interfaces;

namespace BookstoreApp.Cart_management
{
    public class ShoppingCart // Represents a shopping cart
    {   
        private readonly Dictionary<int, CartItem> items = new();

        // Read-only collection of items in the cart
        public IReadOnlyCollection<CartItem> Items => items.Values.ToList();

        // Method to add an item to the cart
        public void Add(IProduct product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");

            if (items.TryGetValue(product.ProductId, out var existing))
                existing.Add(quantity);
            else
                items[product.ProductId] = new CartItem(product, quantity);
        }

        // Method to remove an item from the cart
        public bool Remove(int sku, int quantity)
        {
            if (!items.TryGetValue(sku, out var existing))
                return false;

            if (quantity <= 0) quantity = 1;

            if (quantity >= existing.Quantity)
                items.Remove(sku);
            else
                existing.Remove(quantity);

            return true;
        }

        // Method to clear the cart
        public void Clear() => items.Clear();

        // Method to calculate the total price of items in the cart
        public int Total() =>
            (int)Convert.ToDecimal(items.Values.Sum(i => i.Product.PriceCents * i.Quantity));

        // Method to check if the cart is empty
        public bool IsEmpty() => items.Count == 0;
    }
}

// <summary>
// ShoppingCart class represents a customer's shopping cart in the bookstore
// It manages the items added to the cart and provides methods for calculating totals and clearing the cart
// </summary>