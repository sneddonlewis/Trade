// See https://aka.ms/new-console-template for more information

using Trade.Application;
using Trade.Core;
using Trade.Data.Binance;
using static System.Console;

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(4));

IOrderBookRepo orderBookRepo = new OrderBookBinanceRepo(binanceEndpoint);

var orderBookEnumerable = orderBookRepo.StreamAsync(cts.Token);

try
{
    await foreach (var orderBook in orderBookEnumerable)
    {
        WriteLine("Received");
        WriteLine(orderBook.EventType);
        WriteLine(orderBook.EventTimeStamp);
        WriteLine(orderBook.Symbol);

        WriteLine("Asks");
        orderBook.Asks.Values.ForEach(v => WriteLine($"Ask: {v}"));

        WriteLine("Bids");
        orderBook.Bids.Values.ForEach(v => WriteLine($"Bid: {v}"));
    }

}
catch (OperationCanceledException e)
{
    WriteLine("Finished");
}
