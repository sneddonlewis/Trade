import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderBookService {
  data$ = this.connect()

  constructor() {}

  connect(): Observable<any> {
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

    return new Observable(observer => {
      if (!ws) {
        return;
      }

      ws.onmessage = (event) => {
        console.log(event)
        observer.next(event.data);
      };

      return () => {
        ws?.close();
      };
    });
  }
}
