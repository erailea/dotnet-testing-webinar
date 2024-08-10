using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IExternalBookService _externalBookService;

        public BookService(IBookRepository bookRepository, IExternalBookService externalBookService)
        {
            _bookRepository = bookRepository;
            _externalBookService = externalBookService;
        }

        public IEnumerable<Book> GetAllBooks() => _bookRepository.GetAllBooks();

        public Book GetBookById(int id) => _bookRepository.GetBookById(id);

        public void AddBook(Book book) => _bookRepository.AddBook(book);

        public void UpdateBook(Book book) => _bookRepository.UpdateBook(book);

        public void DeleteBook(int id) => _bookRepository.DeleteBook(id);

        public async Task<Book> GetBookDetailsFromExternalApiAsync(string isbn)
        {
            return await _externalBookService.FetchBookDetailsAsync(isbn);
        }
    }
}
