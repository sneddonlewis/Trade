// See https://aka.ms/new-console-template for more information

using Trade.Core;
using Trade.Data;
using static System.Console;


var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(4));


var orderBookEnumerable = OrderBookDataSources.BinanceBtcUsd.StreamAsync(cts.Token);

try
{
    await foreach (var orderBook in orderBookEnumerable)
    {
        WriteLine("Received");
        WriteLine(orderBook.EventType);
        WriteLine(orderBook.EventTimeStamp);
        WriteLine(orderBook.Symbol);

        WriteLine("Asks");
        orderBook.Asks.ForEach(v => WriteLine($"Ask: {v}"));

        WriteLine("Bids");
        orderBook.Bids.ForEach(v => WriteLine($"Bid: {v}"));
    }

}
catch (OperationCanceledException e)
{
    WriteLine("Finished");
}
