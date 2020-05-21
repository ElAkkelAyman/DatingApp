import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: any = false;
  values: any = {};
    constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    this.getValues();
  }
  RegistrationModeOn() {
 this.registerMode = true;
  }
  getValues(){
    this.http.get('http://localhost:5000/api/values').subscribe(response =>
    {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }
  CancelRegisterMode(){
    console.log('CancelMode!');
    this.registerMode = false;
  }
  loggedIn(){
    // const token = localStorage.getItem('token');
    // return !!token ; // if something in token return true , if not  return false!
    return this.authService.LoggedIn();
  }

}
