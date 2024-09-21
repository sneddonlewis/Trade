using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Trade.Application;
using Trade.Application.Binance;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public OrderBook OrderBook { get; } = new OrderBook();

    public void OnGet()
    {
        Task.Run(FillOrderBook).GetAwaiter().GetResult();
    }

    private async Task FillOrderBook()
    {
        const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(4));

        await OrderBook.ReceiveDataFromBinanceApi(new Uri(binanceEndpoint), cts.Token);
    }
}