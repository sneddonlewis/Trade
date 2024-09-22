using System.Text.Json.Serialization;

namespace Trade.Data.Binance.Models;

public class BinanceOrderDepthDto
{
    [JsonPropertyName("e")]
    public string Event { get; set; }
    
    [JsonPropertyName("E")]
    public long EventTimeStamp { get; set; }
    
    [JsonPropertyName("s")]
    public string Symbol { get; set; }
    
    [JsonPropertyName("a")]
    public string[][] Asks { get; set; }
    
    [JsonPropertyName("b")]
    public string[][] Bids { get; set; }
}