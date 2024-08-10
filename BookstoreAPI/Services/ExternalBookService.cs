using BookstoreAPI.Models;
using System.Text.Json;
using BookstoreAPI.Services;
using System.Drawing;

namespace BookstoreAPI.Services
{
    public class ExternalBookService : IExternalBookService
    {
        private readonly HttpClient _httpClient;

        public ExternalBookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Book> FetchBookDetailsAsync(string isbn)
        {
            var response = await _httpClient.GetStringAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}");

            var bookData = JsonSerializer.Deserialize<JsonElement>(response);

            var volumeInfo = bookData.GetProperty("items")[0].GetProperty("volumeInfo");

            return new Book
            {
                Title = volumeInfo.GetProperty("title").GetString(),
                Author = volumeInfo.GetProperty("authors")[0].GetString(),
                ISBN = isbn,
                Description = volumeInfo.GetProperty("description").GetString()
            };
        }
    }
}

