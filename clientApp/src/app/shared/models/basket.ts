import { v4 as uuid } from 'uuid';

export interface IBasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export interface IBasket {
    id: string;
    items: IBasketItem[];
    clientSecret?: string;
    paymentIntentId?: string;
    deliveryMethodId?: number;
    shippingPrice: number;
}

export class Basket implements IBasket {
    id = uuid();
    items: IBasketItem[] = [];
    shippingPrice: number = 0;
    clientSecret?: string;
    deliveryMethodId?: number;
}

export interface IBasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}