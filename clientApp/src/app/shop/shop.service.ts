import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})

export class ShopService {

  baseUrl = 'https://localhost:7083/api/'

  constructor(private http: HttpClient) { }

  getProducts(shopParams : ShopParams){
    let params = new HttpParams();

    if(shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if(shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageIndex', shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
       .pipe(
          map(response =>{return response.body;})
       );
  }

  getProductBrands(){
    return this.http.get<IProductBrand[]>(this.baseUrl + 'products/brands');
  }

  getProductTypes(){
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types');
  }
}
