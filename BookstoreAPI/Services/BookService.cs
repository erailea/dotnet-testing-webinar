using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IExternalBookHttpClient _externalBookHttpClient;

        public BookService(IBookRepository bookRepository, IExternalBookHttpClient externalBookHttpClient)
        {
            _bookRepository = bookRepository;
            _externalBookHttpClient = externalBookHttpClient;
        }

        public IEnumerable<Book> GetAllBooks() => _bookRepository.GetAllBooks();

        public Book? GetBookById(int id) => _bookRepository.GetBookById(id);

        public void AddBook(Book book) => _bookRepository.AddBook(book);

        public void UpdateBook(Book book) => _bookRepository.UpdateBook(book);

        public void DeleteBook(int id) => _bookRepository.DeleteBook(id);

        public async Task<Book?> GetBookDetailsFromExternalApiAsync(string isbn)
        {
            return await _externalBookHttpClient.FetchBookDetailsAsync(isbn);
        }
    }
}
