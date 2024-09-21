// See https://aka.ms/new-console-template for more information

using Trade.Application;
using Trade.Application.Binance;
using Trade.Core;
using static System.Console;

const string binanceEndpoint = "wss://fstream.binance.com/stream?streams=btcusdt@depth";

OrderBook orderBook = new();

var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(4));

await orderBook.ReceiveDataFromBinanceApi(new Uri(binanceEndpoint), cts.Token);

orderBook.Asks.Values.ForEach(v => WriteLine($"Ask: {v}"));
orderBook.Bids.Values.ForEach(v => WriteLine($"Bid: {v}"));
