// pagination.component.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {
  @Input() totalCount: number = 0;
  @Input() pageSize: number = 6;
  @Output() pageChanged = new EventEmitter<number>();
  @Input()  currentPage: number = 1;

  get pageCount(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  onPageChanged(pageNumber:number ): void {
    if (pageNumber >= 1 && pageNumber <= this.pageCount && pageNumber !== this.currentPage) {
      this.currentPage = 1;
      this.pageChanged.emit(pageNumber);
    }
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.pageCount }, (_, index) => index + 1);
  }
}
