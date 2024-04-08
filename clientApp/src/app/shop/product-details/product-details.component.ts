import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit{
  product!: IProduct;

  constructor(private shopService: ShopService, 
              private activatedRoute: ActivatedRoute, 
              private breadcrumbService: BreadcrumbService){
                this.breadcrumbService.set('@productDetails', '');
              }

  ngOnInit(){
    this.loadProduct();
  }

  loadProduct(){
    this.shopService.getProductDetail(+this.activatedRoute.snapshot.paramMap.get('id')!).subscribe({
      next: (response) => {
        this.product = response;
        this.breadcrumbService.set('@productDetails', this.product.name);
      },
      error: (error) => {
        console.log(error);
      }
    })
  }
}
