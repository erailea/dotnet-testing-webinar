using BookstoreAPI.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BookstoreAPI.Tests
{
    public class BooksControllerIntegrationTests : IClassFixture<BookStoreWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly BookStoreWebApplicationFactory<Program> _factory;
        private readonly BookDbContext _bookDbContext;

        public BooksControllerIntegrationTests(BookStoreWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _bookDbContext = factory.integrationTestDbContext;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/books");
            _bookDbContext.Books = new List<Book>{
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Description = "Description 1" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "1234567891", Description = "Description 2" }
            };
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedResponse()
        {
            //Arrange
            var book = new Book { Title = "New Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            _bookDbContext.Books = new List<Book>();
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/books", content);
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContentResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            _bookDbContext.Books = new List<Book> { book };
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/books/1", content);
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsBadRequestResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            _bookDbContext.Books = new List<Book> { book };
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/books/2", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsInternalServerErrorResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            _bookDbContext.Books = null;
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/books/1", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFoundResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            var existingBook = new Book { Id = 2, Title = "Updated Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
            _bookDbContext.Books = new List<Book> { existingBook };
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/books/1", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContentResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Description = "Description 1" };
            _bookDbContext.Books = new List<Book> { book };
            //Act
            var response = await _client.DeleteAsync("/api/books/1");
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFoundResponse()
        {
            //Arrange            
            _bookDbContext.Books = new List<Book>();
            //Act
            var response = await _client.DeleteAsync("/api/books/1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ReturnsInternalServerErrorResponse()
        {
            //Arrange            
            _bookDbContext.Books = null;
            //Act
            var response = await _client.DeleteAsync("/api/books/1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResponse()
        {
            //Arrange
            var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", Description = "Description 1" };
            _bookDbContext.Books = new List<Book> { book };
            //Act
            var response = await _client.GetAsync("/api/books/1");
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFoundResponse()
        {
            //Arrange
            _bookDbContext.Books = new List<Book>();
            //Act
            var response = await _client.GetAsync("/api/books/1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task FetchBookDetails_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("")
            };

            _factory.handlerMock
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(responseMessage);

            // Act
            var response = await _client.GetAsync("/api/books/external/1234567890");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task FetchBookDetails_ReturnsNotFoundResponse()
        {
            // Arrange
            var json = @"
            {
                ""items"": [
                ]
            }";

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _factory.handlerMock
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(responseMessage);

            // Act
            var response = await _client.GetAsync("/api/books/external/1234567890");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task FetchBookDetails_ReturnsOkResponse()
        {
            // Arrange
            var json = @"
                    {
                        ""items"": [
                            {
                                ""volumeInfo"": {
                                    ""title"": ""Book 1"",
                                    ""authors"": [""Author 1""],
                                    ""description"": ""Description 1""
                                }
                            }
                        ]
                    }";

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _factory.handlerMock
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                       ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(responseMessage);

            // Act
            var response = await _client.GetAsync("/api/books/external/1234567890");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
