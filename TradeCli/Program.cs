// See https://aka.ms/new-console-template for more information

using Trade.Application.Binance;
using Trade.Core;
using static System.Console;

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";


var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(4));

var orderBookEnumerable = OrderBookBinanceData.ReceiveDataFromBinanceApi(new Uri(binanceEndpoint), cts.Token);

await foreach (var orderBook in orderBookEnumerable)
{
    WriteLine("Received");
    WriteLine(orderBook.Event);
    WriteLine(orderBook.EventTimeStamp);
    WriteLine(orderBook.Symbol);

    WriteLine("Asks");
    orderBook.Asks.Values.ForEach(v => WriteLine($"Ask: {v}"));

    WriteLine("Bids");
    orderBook.Bids.Values.ForEach(v => WriteLine($"Bid: {v}"));
}
