using Everys.BL.StockApiModels;

namespace Everys.BL.Interfaces
{
    public interface IStockService
    {
        Task<StockApiResponseModel> GetStockAsync(string? filter = null, int? skip = null, int? take = null, string? expand = null, string? orderBy = null, string? orderDirection = null);
    }
}
