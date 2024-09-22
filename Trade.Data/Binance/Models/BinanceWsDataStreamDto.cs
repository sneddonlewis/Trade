using System.Text.Json.Serialization;

namespace Trade.Data.Binance.Models;

public class BinanceWsDataStreamDto
{
    [JsonPropertyName("stream")]
    public string Stream { get; init; }
    
    [JsonPropertyName("data")]
    public BinanceOrderDepthDto Data { get; init; }
}