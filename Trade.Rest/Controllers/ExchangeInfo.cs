using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Trade.Core;

namespace Trade.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeInfo(HttpClient httpClient) : ControllerBase
{
    [HttpGet]
    public async Task<BinanceExchangeInfo> Markets()
    {
        const string api = "https://api.binance.com/api/v3/exchangeInfo?symbols=%5B%22BNBBTC%22,%22BTCUSDT%22%5D";
        var response = await httpClient.GetAsync(api);
        Console.WriteLine($"Status from Binance {response.StatusCode}");
        
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        
        var info = JsonSerializer.Deserialize<BinanceExchangeInfo>(content);
        var firstSymbol = info.Symbols.First();
        Console.WriteLine(firstSymbol.Symbol);
        return info;
    }
    
}

public record Market
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
}

public record BinanceExchangeInfo
{
    [JsonPropertyName("serverTime")]
    public long ServerTime { get; set; }
    
    [JsonPropertyName("symbols")]
    public Market[] Symbols { get; set; }
}