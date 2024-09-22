import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderBookService } from './order-book.service';



@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    OrderBookService,
  ]
})
export class ServicesModule { }
