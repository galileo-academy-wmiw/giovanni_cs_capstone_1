namespace BookstoreApp.Products;

public class AudioBook : Book
{
    public double FileSize { get; set; }

    public override string Type => "Audiobook";

    public AudioBook() { }

    public AudioBook(string title, int id, int year, string genre, double fileSize, int price = 0)
        : base(title, id, year, genre, price)
    {
        FileSize = fileSize;
    }
}

// <summary>
// AudioBook class represents an audiobook in the bookstore
// It contains properties for the audiobook's file size and overrides methods to display its details
// This class is used to manage audiobook information and provide specific functionality for audiobooks
// </summary>