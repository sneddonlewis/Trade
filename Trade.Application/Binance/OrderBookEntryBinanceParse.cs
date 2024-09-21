namespace Trade.Application.Binance;

public static class OrderBookEntryBinanceParse
{
    public static IEnumerable<OrderBookEntry> ToOrderBookEntry(this string[][] matrix) =>
        matrix.Select(rawEntry => new OrderBookEntry(rawEntry.ParsePrice(), rawEntry.ParseQuantity()));

    private static decimal ParsePrice(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
    
    private static decimal ParseQuantity(this string[] rawEntry) => decimal.Parse(rawEntry[0]);
}