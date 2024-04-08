import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IProductBrand } from '../shared/models/product-brand';
import { IProductType } from '../shared/models/product-type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm!: ElementRef;
  products: IProduct[] = [];
  productBrands: IProductBrand[] = [];
  productTypes: IProductType[] = [];
  shopParams = new ShopParams();
  totalCount: number = 0;
  sortOptions=[
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'},
  ]

  constructor(private shopService: ShopService){}

  ngOnInit(){
    this.getProducts();
    this.getProductBrands();
    this.getProductTypes();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        if(response != null){
          this.products = response.data;
          this.shopParams.pageNumber = response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        }
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  getProductBrands(){
    this.shopService.getProductBrands().subscribe({
      next: (response) => {
        this.productBrands = [{id:0, name:'All'}, ...response];
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  getProductTypes(){
    this.shopService.getProductTypes().subscribe({
      next: (response) => {
        this.productTypes = [{id:0, name:'All'}, ...response];
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  onBrandSelected(brandId: number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1
    this.getProducts();
  }

  onSortSelected(sort: string){
    this.shopParams.sort = sort;
    this.getProducts()
  }

  onPageChanged(event: number){
    this.shopParams.pageNumber = event;
    this.getProducts();
  }

  onSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1
    this.getProducts();
  }

  onReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
