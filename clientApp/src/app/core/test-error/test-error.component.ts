import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit{
  baseUrl = environment.apiUrl
  validationErrors: any;
  constructor(private http: HttpClient){}

  ngOnInit(){
  }

  get404error(){
    this.http.get(this.baseUrl + 'Products/100000').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error: (error) =>{
        console.log(error);
      }
    });
  }
  get500error(){
    this.http.get(this.baseUrl + 'Products/error').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error: (error) =>{
        console.log(error);
      }
    });
  }

  get400error(){
    this.http.get(this.baseUrl + 'Products/error400').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error: (error) =>{
        console.log(error);
      }
    });
  }

  get400Validationerror(){
    this.http.get(this.baseUrl + 'Products/error400/multi').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error: (error) =>{
        this.validationErrors = error.errors;
      }
    });
  }
}
