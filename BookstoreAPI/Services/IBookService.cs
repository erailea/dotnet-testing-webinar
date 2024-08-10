using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
        Task<Book?> GetBookDetailsFromExternalApiAsync(string isbn);
    }
}
