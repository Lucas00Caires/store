import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket | null>(null);
  private baskerTotalSource = new BehaviorSubject<IBasketTotals | null>(null)
  basketTotal$ = this.baskerTotalSource.asObservable();
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string){
    return this.http.get(this.baseUrl + 'basket?id=' + id)
    .pipe(
      map((response: any)=> {
        const basket: IBasket = response as IBasket;
        this.basketSource.next(basket);
        this.calculateTotals();
      })
    );
  }

  setBasket(basket: IBasket){
    const url = `${this.baseUrl}basket`;
    return this.http.post<IBasket>(url, basket).pipe(
      map((response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
        return response;
      })
    ).subscribe({
      next: (response: IBasket) => {
        console.log('Basket set successfully:', response);
      },
      error: (error) => {
        console.log('Error setting basket:', error);
      }
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue() as Basket;
    const foundItemIndex = basket.items.findIndex(x => x.id == item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue() as Basket;
    const foundItemIndex = basket.items.findIndex(x => x.id == item.id);
    if( basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else{
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket?.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id);
      if(basket.items.length > 0)
        this.setBasket(basket);
      else
        this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe( () => {
      this.basketSource.next(null);
      this.baskerTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error =>{
      console.log(error);
    });
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if(index === -1){
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }else{
      items[index].quantity += quantity;
    }
    return items;
  }
  
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }
;

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    }
  }

  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket?.items.reduce((a, b) => (b.price * b.quantity) + a, 0) ?? 0;
    const total = subtotal + shipping;
    this.baskerTotalSource.next({shipping, total, subtotal})
  }
}
