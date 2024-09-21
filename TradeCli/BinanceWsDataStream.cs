using System.Text.Json.Serialization;

namespace TradeCli;

public class BinanceWsDataStream
{
    [JsonPropertyName("stream")]
    public string Stream { get; set; }
    
    [JsonPropertyName("data")]
    public BinanceOrderDepthResult Data { get; set; }
}