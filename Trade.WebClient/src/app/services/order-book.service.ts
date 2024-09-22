import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderBook } from '../types/order-book';

@Injectable({
  providedIn: 'root'
})
export class OrderBookService {
  data$ = this.connect()

  constructor() {}

  connect(): Observable<OrderBook> {
    const ws = new WebSocket('wss://localhost:7135/order_book');

    ws.onopen = () => {
      console.log('WebSocket connected');
    };

    ws.onclose = () => {
      console.log('WebSocket closed');
    };

    ws.onerror = (error) => {
      console.error('WebSocket error:', error);
    };

    return new Observable<OrderBook>(observer => {
      if (!ws) {
        return;
      }

      ws.onmessage = (event) => {
        const parsedData = JSON.parse(event.data)
        console.log(parsedData)
        observer.next(parsedData);
      };

      return () => {
        ws?.close();
      };
    });
  }
}
