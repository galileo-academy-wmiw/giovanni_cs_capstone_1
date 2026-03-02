namespace BookstoreApp.Products
{
    public class EBook : Book
    {
        public double FileSize { get; set; } 

        public override string Type => "eBook";

        public EBook() { }

        public EBook(string title, int id, int year, string genre, double fileSize, int price = 0)
            : base(title, id, year, genre, price)
        {
            FileSize = fileSize;
        }
        
    }
}

// <summary>
// EBook class represents an electronic book in the bookstore
// It contains properties for the ebook's file size and overrides methods to display its details
// This class is used to manage ebook information and provide specific functionality for ebooks
// </summary>