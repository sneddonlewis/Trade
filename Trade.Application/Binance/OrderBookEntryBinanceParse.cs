namespace Trade.Application.Binance;

public static class OrderBookEntryBinanceParse
{
    public static IEnumerable<OrderBookEntry> ToOrderBookEntry(this string[][] matrix) =>
        matrix.Select(rawEntry => new OrderBookEntry(rawEntry.ParsePrice(), rawEntry.ParseQuantity()));

    public static OrderBook ToOrderBook(this BinanceOrderDepthResult binanceResult)
    {
        var book = new OrderBook()
        {
            Event = binanceResult.Event,
            EventTimeStamp = binanceResult.EventTimeStamp,
            Symbol = binanceResult.Symbol,
            
        };
        book.InsertAsks(binanceResult.Asks.ToOrderBookEntry());
        book.InsertBids(binanceResult.Bids.ToOrderBookEntry());
        return book;
    }

    private static decimal ParsePrice(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
    
    private static decimal ParseQuantity(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
}