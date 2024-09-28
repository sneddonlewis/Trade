using System.Text.Json.Serialization;

namespace TradeCli;

public class BinanceKlineDataDto
{
    [JsonPropertyName("e")]
    public string EventType { get; set; }
    
    [JsonPropertyName("E")]
    public long EventTimestamp { get; set; }
    
    [JsonPropertyName("s")]
    public string TickerSymbol { get; set; }
    
    [JsonPropertyName("k")]
    public BinanceKlineDto Kline { get; set; }
}

public class BinanceKlineDto
{
    [JsonPropertyName("o")]
    public string Open { get; set; }
    
    [JsonPropertyName("h")]
    public string High { get; set; }
    
    [JsonPropertyName("l")]
    public string Low { get; set; }
    
    [JsonPropertyName("c")]
    public string Close { get; set; }
}