using Everys.BL.Exceptions;
using Everys.BL.Interfaces;
using Everys.BL.StockApiModels;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace Everys.BL.Services
{
    public class StockService : IStockService
    {
        private const string API_URI = "http://fakestock.everys.com/api";
        private readonly HttpClient _httpClient;

        public StockService()
        {
            _httpClient = new();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                .AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"candidate:candidate321")));
        }

        public async Task<StockApiResponseModel> GetStockAsync(string? filter = null, int? skip = null, int? take = null, string? expand = null, string? orderBy = null, string? orderDirection = null)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(skip?.ToString()))
                parameters[nameof(skip)] = skip?.ToString();
            if (!string.IsNullOrWhiteSpace(take?.ToString()))
                parameters[nameof(take)] = take?.ToString();
            if (!string.IsNullOrWhiteSpace(expand))
                parameters[nameof(expand)] = expand;
            if (!string.IsNullOrWhiteSpace(filter))
                parameters[nameof(filter)] = filter;
            if (!string.IsNullOrWhiteSpace(orderBy))
                parameters[nameof(orderBy)] = orderBy;
            if (!string.IsNullOrWhiteSpace(orderDirection))
                parameters[nameof(orderDirection)] = orderDirection;

            var url = $"{API_URI}/v1/Stock?{parameters}";

            var serviceResponse = await _httpClient
             .GetFromJsonAsync<StockApiResultResponseModel>(url);

            if (serviceResponse?.Result?.Items is null)
            {
                throw new OurUserFriendlyException("API is not available.");
            }

            return serviceResponse.Result;
        }
    }
}
