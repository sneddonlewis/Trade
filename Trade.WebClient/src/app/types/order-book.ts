export interface OrderBookEntry {
  price: number;
  quantity: number;
}

export interface OrderBook {
  eventType: string,
  eventTimeStamp: number,
  symbol: string,
  bids: OrderBookEntry[],
  asks: OrderBookEntry[],
}
