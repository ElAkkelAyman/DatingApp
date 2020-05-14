import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {
 @Input() valuesFromHome: any;
 @Output() cancelRegister = new EventEmitter();
model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  Register(){
    this.authService.Register(this.model).subscribe(next => {
      console.log('successfuly registered') ;
     }, error => {
      console.log(error);
     });
    console.log(this.model);
  }
  Cancel() {
    this.cancelRegister.emit();
    console.log('cancel register !');
  }

}
