using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public class BooksClient : IBooksClient
    {
        private readonly string _apiBaseUrl;
        private readonly IHttpClientFactory _clientFactory;

        //todo: use IOptions<TConfig>
        public BooksClient(string apiBaseUrl, IHttpClientFactory clientFactory)
        {
            _apiBaseUrl = apiBaseUrl;
            _clientFactory = clientFactory;
        }

        public Task<Response<SearchBooksResult>> Get(string pattern, int skipCount, int takeCount)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<Book>> Create(CreateBookModel bookViewModel)
        {
            var c1 = _clientFactory.CreateClient();
            var a = await c1.GetDiscoveryDocumentAsync("https://localhost:44338/");
            var tokenResponse = await c1.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = a.TokenEndpoint,
                ClientId = "my_client1_id",
                ClientSecret = "my_client1_secret".ToSha256(),
                Scope = "MyApi2"
            });

            var c2 = _clientFactory.CreateClient();

            c2.SetBearerToken(tokenResponse.AccessToken);

            c2.BaseAddress = new Uri(_apiBaseUrl);

            //var ad = c2.PostAsync("books", new ReadOnlyMemoryContent())


            return null;
        }

        public Task<Response<Book>> GetBook(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Book>> UpdateBook(UpdateBookModel bookViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Book>> SetRate(SetRateModel setRateViewModel)
        {
            throw new NotImplementedException();
        }
    }
}