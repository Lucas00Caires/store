import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PaginationComponent } from './components/pagination/pagination.component';


@NgModule({
  declarations: [
    PagingHeaderComponent,
    PaginationComponent
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    PagingHeaderComponent,
    PaginationComponent
  ]
})
export class SharedModule { }
