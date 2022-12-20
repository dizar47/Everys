using System.Text.Json.Serialization;

namespace Everys.BL.StockApiModels
{
    public class StockApiResponseModel
    {
        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public StockItemModel[] Items { get; set; }

        [JsonPropertyName("receivedUtc")]
        public DateTime ReceivedUtc { get; set; } = DateTime.UtcNow;
    }
}
