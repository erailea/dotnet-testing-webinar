using BookstoreAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace BookstoreAPI.Tests
{

    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public BookDbContext integrationTestDbContext { get; private set; } = new BookDbContext();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(integrationTestDbContext);
            });
        }
    }


    public class BooksControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly BookDbContext _bookDbContext;

        public BooksControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _bookDbContext = factory.integrationTestDbContext;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/books");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotNull(responseString);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedResponse()
        {
            //Arrange
            var book = new Book { Title = "New Book", Author = "Author", ISBN = "1234567890", Description = "Description" };
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
            _bookDbContext.Books.Add(book);
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
            _bookDbContext.Books.Add(book);
            var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync("/api/books/1", content);
            response.EnsureSuccessStatusCode();
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
