using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IExternalBookService
    {
        Task<Book> FetchBookDetailsAsync(string isbn);
    }
}
