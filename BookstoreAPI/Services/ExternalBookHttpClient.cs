using BookstoreAPI.Models;
using System.Text.Json;
using BookstoreAPI.Services;
using System.Drawing;

namespace BookstoreAPI.Services
{
    public class ExternalBookHttpClientException : Exception
    {
        public ExternalBookHttpClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class ExternalBookHttpClient : IExternalBookHttpClient
    {
        private readonly HttpClient _httpClient;

        public ExternalBookHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Book?> FetchBookDetailsAsync(string isbn)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}");

                var bookData = JsonSerializer.Deserialize<JsonElement>(response);

                var items = bookData.GetProperty("items");

                if (items.GetArrayLength() == 0)
                {
                    return null;
                }

                var volumeInfo = items[0].GetProperty("volumeInfo");

                return new Book
                {
                    Title = volumeInfo.GetProperty("title").GetString(),
                    Author = volumeInfo.GetProperty("authors")[0].GetString(),
                    ISBN = isbn,
                    Description = volumeInfo.GetProperty("description").GetString()
                };
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalBookHttpClientException("Failed to fetch book details from external API", ex);
            }
        }
    }
}

