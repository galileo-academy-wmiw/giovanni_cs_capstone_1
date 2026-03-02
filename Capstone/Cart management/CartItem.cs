using BookstoreApp.Interfaces;

namespace BookstoreApp.Cart_management
{
    public class CartItem
    {
        public IProduct Product { get; }
        public int Quantity { get; private set; }

        // Constructor for CartItem
        public CartItem(IProduct product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        // Method to add quantity to the cart item
        public void Add(int qty) => Quantity += qty;

        // Method to remove quantity from the cart item
        public void Remove(int qty)
        {
            Quantity -= qty;
            if (Quantity < 0) Quantity = 0;
        }
    }
}

// <summary>
// CartItem class represents an item in the shopping cart
// It holds the product information and its quantity in the cart
// It provides methods to add or remove quantities from the cart item
// It ensures that the quantity cannot go below zero
// It also holds a reference to the product being purchased
// This class is used by the ShoppingCart class to manage items in the cart
// </summary>