using Everys.BL.Exceptions;
using Everys.BL.Interfaces;
using Everys.BL.StockApiModels;
using Everys.BL.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Everys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductServiceController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductServiceController> _logger;
        private readonly IStockService _stockService;

        public ProductServiceController(IDistributedCache cache, IStockService stockService, ILogger<ProductServiceController> logger)
        {
            _cache = cache;
            _logger = logger;
            _stockService = stockService;
        }

        [HttpGet("get/name")]
        public async Task<ApiResponseBase> GetByTitle(string? title, int? skip, int? take, string? expand, string? orderBy, string? orderDirection)
        {
            return await GetData(
                    string.IsNullOrWhiteSpace(title) ? default : ApplyFilterPattern(nameof(title), title),
                    skip, take, expand, orderBy, orderDirection);
        }

        [HttpGet("get/description")]
        public async Task<ApiResponseBase> GetByDescription(string? description, int? skip, int? take, string? expand, string? orderBy, string? orderDirection)
        {
            return await GetData(
                    string.IsNullOrWhiteSpace(description) ? default : ApplyFilterPattern(nameof(description), description),
                    skip, take, expand, orderBy, orderDirection);
        }

        [HttpGet("get/manufacturer")]
        public async Task<ApiResponseBase> GetByManufacturer(string? manufacturer, int? skip, int? take, string? expand, string? orderBy, string? orderDirection)
        {
            return await GetData(
                    string.IsNullOrWhiteSpace(manufacturer) ? default : ApplyFilterPattern(nameof(manufacturer), manufacturer),
                    skip, take, expand, orderBy, orderDirection);
        }

        private async Task<ApiResponseBase> GetData(string? filter, int? skip, int? take, string? expand, string? orderBy, string? orderDirection)
        {
            var key = GetKey(filter, skip, take, expand, orderBy, orderDirection);

            try
            {
                var response = await _stockService.GetStockAsync(filter, skip, take, expand, orderBy, orderDirection);

                await _cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(response));

                return new ApiResponse<StockApiResponseModel>(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetData");

                var cachedResponseBytes = await _cache.GetAsync(key);

                if (cachedResponseBytes != null)
                {
                    using var stream = new MemoryStream(cachedResponseBytes);

                    var cachedResponse = await JsonSerializer.DeserializeAsync<StockApiResponseModel>(stream);

                    return new ApiResponse<StockApiResponseModel>(cachedResponse, true);
                }
                else
                {
                    var ufe = e as OurUserFriendlyException;

                    return ufe is null
                        ? new BadApiResponse()
                        : new BadApiResponse(ufe);
                }
            }
        }

        private string ApplyFilterPattern(string field, string value)
        {
            return string.Format("{1}", field, value); // What is the right pattern ? '{0}={1}' doesn't work
        }

        private string GetKey(string? filter, int? skip, int? take, string? expand, string? orderBy, string? orderDirection)
        {
            const string separator = "###";

            return string.Concat(filter, separator, skip, separator, take, separator, expand, separator, orderBy, separator, orderDirection);
        }
    }
}