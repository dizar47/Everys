using System.Text.Json.Serialization;

namespace Everys.BL.StockApiModels
{
    public class StockApiResultResponseModel
    {
        [JsonPropertyName("result")]
        public StockApiResponseModel Result { get; set; }
    }
}
