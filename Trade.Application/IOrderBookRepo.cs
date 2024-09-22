using Trade.Core.Models;

namespace Trade.Application;

public interface IOrderBookRepo
{
    IAsyncEnumerable<OrderBook> StreamAsync(CancellationToken cancellationToken);
}