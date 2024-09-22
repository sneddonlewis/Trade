using Trade.Core;
using Trade.Core.Models;
using Trade.Data.Binance.Models;

namespace Trade.Data.Binance;

public static class OrderBookBinanceParse
{
    public static OrderBook ToOrderBook(this BinanceOrderDepthDto binanceDto)
    {
        var highestFirstComparer = Comparer<decimal>.Create((prev, next) => prev.CompareTo(next));
        
        SortedDictionary<decimal, OrderBookEntry> bids = new(highestFirstComparer);
        SortedDictionary<decimal, OrderBookEntry> asks = new();
        
        binanceDto.Asks.ToOrderBookEntry().ForEach(e => asks[e.Price] = e);
        binanceDto.Bids.ToOrderBookEntry().ForEach(e => bids[e.Price] = e);
        
        var book = new OrderBook(binanceDto.Event, binanceDto.EventTimeStamp, binanceDto.Symbol, asks, bids);
        
        return book;
    }
    
    private static IEnumerable<OrderBookEntry> ToOrderBookEntry(this string[][] matrix) =>
        matrix.Select(rawEntry => new OrderBookEntry(rawEntry.ParsePrice(), rawEntry.ParseQuantity()));

    private static decimal ParsePrice(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
    
    private static decimal ParseQuantity(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
}