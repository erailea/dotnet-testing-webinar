using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IExternalBookHttpClient
    {
        Task<Book?> FetchBookDetailsAsync(string isbn);
    }
}
