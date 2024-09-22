import { Component, } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { OrderBookService } from './services/order-book.service';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AsyncPipe, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Trade.WebClient';

  constructor(public orderBookService: OrderBookService) {}
}
