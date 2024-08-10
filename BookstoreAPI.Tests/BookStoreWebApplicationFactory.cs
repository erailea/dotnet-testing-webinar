using BookstoreAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BookstoreAPI.Tests
{
    public class BookStoreWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public BookDbContext integrationTestDbContext { get; private set; } = new BookDbContext();
        public Mock<HttpMessageHandler> handlerMock { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(integrationTestDbContext);

                var httpClientFactoryDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                        typeof(IHttpClientFactory));
                services.Remove(httpClientFactoryDescriptor);

                var httpClientDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                        typeof(HttpClient));
                services.Remove(httpClientDescriptor);

                this.handlerMock = new Mock<HttpMessageHandler>();

                var client = new HttpClient(this.handlerMock.Object);
                var clientFactoryMock = new Mock<IHttpClientFactory>();
                clientFactoryMock
                    .Setup(f => f.CreateClient(It.IsAny<string>()))
                    .Returns(client);

                services.AddSingleton(clientFactoryMock.Object);
                services.AddSingleton(client);
            });
        }
    }
}
