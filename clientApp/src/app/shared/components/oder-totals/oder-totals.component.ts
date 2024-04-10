import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasketTotals } from '../../models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-oder-totals',
  templateUrl: './oder-totals.component.html',
  styleUrls: ['./oder-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit{
  basketTotal$!: Observable<IBasketTotals | null>;

  constructor(private basketService: BasketService){}

  ngOnInit(){
    this.basketTotal$ = this.basketService.basketTotal$;
  }

}
