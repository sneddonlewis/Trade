import { Component, } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OrderBookService } from './services/order-book.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { OrderBookEntry } from './types/order-book';
import { KlineService } from './services/kline.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AsyncPipe, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Trade.WebClient';

  constructor(
    public orderBookService: OrderBookService,
    public klineService: KlineService
  ) {}

  volume = (entries: OrderBookEntry[]): number =>
    entries.map(e => e.quantity).reduce((prev, next) => prev + next)
}
