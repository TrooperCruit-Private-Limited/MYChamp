 using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace MYChamp.Pages.Shared
{
    public class _Layout
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public _Layout(IHttpContextAccessor httpContextAccesor, IConfiguration configuration, IHttpClientFactory httpClientFactory) {
            _httpContextAccessor = httpContextAccesor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
    }
}
