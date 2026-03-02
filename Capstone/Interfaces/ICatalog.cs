namespace BookstoreApp.Interfaces
{
    public interface ICatalogRepository
    {
        IReadOnlyList<IProduct> GetAll();
        
        IProduct? GetById(int id);
        
        bool RemoveById(int id);
        
        bool Add(IProduct product);

        //DB-side filter/sort
        IReadOnlyList<IProduct> Query(string? typeFilter, bool sortByPrice, bool ascending);
    }
}

// <summary>
// Used to communicate functions to the UI
// </summary>