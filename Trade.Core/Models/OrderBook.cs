
namespace Trade.Core.Models;

public readonly record struct OrderBook(
    string EventType,
    long EventTimeStamp,
    string Symbol,
    IDictionary<decimal, OrderBookEntry> Bids,
    IDictionary<decimal, OrderBookEntry> Asks);