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
    public async Task<Market[]> Markets()
    {
        const string api = "https://api.binance.com/api/v3/exchangeInfo";
        var response = await httpClient.GetAsync(api);
        Console.WriteLine($"Status from Binance {response.StatusCode}");
        
        string content = await response.Content.ReadAsStringAsync();
        
        var info = JsonSerializer.Deserialize<BinanceExchangeInfo>(content);
        var firstSymbol = info.Symbols.First();
        Console.WriteLine(firstSymbol.Symbol);
        return info.Symbols;
    }
    
}

public readonly record struct Market
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; }
}

public readonly record struct BinanceExchangeInfo
{
    [JsonPropertyName("symbols")]
    public Market[] Symbols { get; }
}