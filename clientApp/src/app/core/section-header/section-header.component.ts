import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit{
  breadcumb$!: Observable<any[]>;
  constructor(private breadcrumbsService: BreadcrumbService) {}

  ngOnInit(){
    this.breadcumb$ = this.breadcrumbsService.breadcrumbs$;
  }
}
