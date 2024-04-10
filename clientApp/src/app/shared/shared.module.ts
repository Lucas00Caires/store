import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { OrderTotalsComponent } from './components/oder-totals/oder-totals.component';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PaginationComponent,
    OrderTotalsComponent,
  ],
  imports: [
    CommonModule,
    CarouselModule
  ],
  exports: [
    PagingHeaderComponent,
    PaginationComponent,
    CarouselModule,
    OrderTotalsComponent
  ]
})
export class SharedModule { }
