using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services;
using Moq;
using Xunit;

namespace BookstoreAPI.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IExternalBookHttpClient> _externalBookHttpClientMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _externalBookHttpClientMock = new Mock<IExternalBookHttpClient>();
            _bookService = new BookService(_bookRepositoryMock.Object, _externalBookHttpClientMock.Object);
        }

        [Fact]
        public void GetAllBooks_ReturnsBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            _bookRepositoryMock.Setup(repo => repo.GetAllBooks()).Returns(books);

            // Act
            var result = _bookService.GetAllBooks();

            // Assert
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetBookDetailsFromExternalApi_ReturnsBookDetails()
        {
            // Arrange
            var isbn = "1234567890";
            var book = new Book { Title = "External Book", ISBN = isbn };
            _externalBookHttpClientMock.Setup(service => service.FetchBookDetailsAsync(isbn)).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookDetailsFromExternalApiAsync(isbn);

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public void AddBook_BookAdded()
        {
            // Arrange
            var book = new Book { Title = "Test Book" };

            // Act
            _bookService.AddBook(book);

            // Assert
            _bookRepositoryMock.Verify(repo => repo.AddBook(book), Times.Never);
        }

        [Fact]
        public void UpdateBook_BookUpdated()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };

            // Act
            _bookService.UpdateBook(book);

            // Assert
            _bookRepositoryMock.Verify(repo => repo.UpdateBook(book), Times.Once);
        }

        [Fact]
        public void DeleteBook_BookDeleted()
        {
            // Arrange
            var id = 1;

            // Act
            _bookService.DeleteBook(id);

            // Assert
            _bookRepositoryMock.Verify(repo => repo.DeleteBook(id), Times.Once);
        }
    }
}
