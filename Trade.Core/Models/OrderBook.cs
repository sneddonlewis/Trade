
namespace Trade.Core.Models;

public readonly record struct OrderBook(
    string EventType,
    long EventTimeStamp,
    string Symbol,
    IEnumerable<OrderBookEntry> Bids,
    IEnumerable<OrderBookEntry> Asks);