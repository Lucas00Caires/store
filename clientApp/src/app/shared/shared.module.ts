import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PaginationComponent,
  ],
  imports: [
    CommonModule,
    CarouselModule
  ],
  exports: [
    PagingHeaderComponent,
    PaginationComponent,
    CarouselModule,
  ]
})
export class SharedModule { }
