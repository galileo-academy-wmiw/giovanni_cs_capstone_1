using System.ComponentModel.DataAnnotations.Schema;
using BookstoreApp.Interfaces;

namespace BookstoreApp.Products
{
    public class Book : IProduct
    {
        // Core book fields
        public string Title { get; set; } = "";
        
        public int ProductId { get; set; }
        
        public int PublicationYear { get; set; }
        
        public string Genre { get; set; } = "";

        [NotMapped]
        public string Name => Title;

        public int PriceCents { get; set; }

        //helpful for display/UI
        [NotMapped]
        public virtual string Type => "Book";

        public Book() { }

        protected Book(string title, int id, int year, string genre, int price = 0)
        {
            Title = title;
            ProductId = id;
            PublicationYear = year;
            Genre = genre;
            PriceCents = price;
        }
    }
}   

// <summary>
// Book class represents a book in the bookstore
// It contains properties for the book's title, ID, publication year, genre, and price
// This class is used to manage book information and provide functionality for displaying book details
// </summary>