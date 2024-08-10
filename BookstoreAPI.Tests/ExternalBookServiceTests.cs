using System.Net;
using BookstoreAPI.Services;

namespace BookstoreAPI.Tests
{

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _responseMessage;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseMessage);
        }
    }



    public class ExternalBookServiceTests
    {
        [Fact]
        public async Task FetchBookDetailsAsync_ReturnsBook()
        {
            // Arrange
            var jsonResponse = @"
        {
            ""items"": [
                {
                    ""volumeInfo"": {
                        ""title"": ""The Great Gatsby"",
                        ""authors"": [""F. Scott Fitzgerald""],
                        ""description"": ""A classic novel.""
                    }
                }
            ]
        }";

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            var handler = new MockHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(handler);

            var externalBookService = new ExternalBookService(httpClient);

            // Act
            var book = await externalBookService.FetchBookDetailsAsync("1234567890");

            // Assert
            Assert.NotNull(book);
            Assert.Equal("The Great Gatsby", book.Title);
            Assert.Equal("F. Scott Fitzgerald", book.Author);
            Assert.Equal("1234567890", book.ISBN);
            Assert.Equal("A classic novel.", book.Description);
        }

        [Fact]
        public async Task FetchBookDetailsAsync_ThrowsException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var handler = new MockHttpMessageHandler(responseMessage);
            var httpClient = new HttpClient(handler);

            var externalBookService = new ExternalBookService(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => externalBookService.FetchBookDetailsAsync("1234567890"));
        }
    }
}
