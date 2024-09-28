import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { KlineData } from '../types/kline.types';

@Injectable({
  providedIn: 'root'
})
export class KlineService {
  data$ = this.connect()

  constructor() {}

  connect(): Observable<KlineData> {
    const ws = new WebSocket('wss://localhost:7135/kline');

    ws.onopen = () => {
      console.log('WebSocket connected');
    };

    ws.onclose = () => {
      console.log('WebSocket closed');
    };

    ws.onerror = (error) => {
      console.error('WebSocket error:', error);
    };

    return new Observable<KlineData>(observer => {
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
