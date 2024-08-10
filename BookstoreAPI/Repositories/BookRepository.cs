using BookstoreAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookstoreAPI.Repositories
{
    public class BookNotFoundException : System.Exception
    {
        public BookNotFoundException(string message) : base(message) { }
    }
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext bookDbContext;
        public BookRepository(BookDbContext bookDbContext)
        {
            this.bookDbContext = bookDbContext;
        }

        public IEnumerable<Book> GetAllBooks() => bookDbContext.Books;

        public Book GetBookById(int id) => bookDbContext.Books.Find(b => b.Id == id);

        public void AddBook(Book book)
        {
            book.Id = bookDbContext.Books.Count + 1;
            bookDbContext.Books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.Description = book.Description;
            }
            else throw new BookNotFoundException($"Book with id {book.Id} not found");
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                bookDbContext.Books.Remove(book);
            }
            else throw new BookNotFoundException($"Book with id {book.Id} not found");
        }
    }
}
