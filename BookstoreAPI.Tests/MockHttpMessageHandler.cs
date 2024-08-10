using System.Net;
using System.Text;

namespace BookstoreAPI.Tests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _responseMessage;

        public MockHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public MockHttpMessageHandler()
        {
            _responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("json", Encoding.UTF8, "application/json")
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseMessage);
        }
    }
}
