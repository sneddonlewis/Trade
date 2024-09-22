using Trade.Core;

namespace Trade.Application;

public class OrderBook
{
    private static readonly Comparer<decimal> HighestFirstComparer =
        Comparer<decimal>.Create((prev, next) => prev.CompareTo(next));

    public string Event { get; set; }
    
    public long EventTimeStamp { get; set; }
    
    public string Symbol { get; set; }
    public SortedDictionary<decimal, OrderBookEntry> Bids { get; } = new(HighestFirstComparer);
    public SortedDictionary<decimal, OrderBookEntry> Asks { get; } = new();

    public void InsertAsks(IEnumerable<OrderBookEntry> entries) => entries.ForEach(InsertAsk);
    
    public void InsertBids(IEnumerable<OrderBookEntry> entries) => entries.ForEach(InsertBid);
    
    public void InsertAsk(OrderBookEntry entry) => Asks[entry.Price] = entry;
    
    public void InsertBid(OrderBookEntry entry) => Bids[entry.Price] = entry;
}