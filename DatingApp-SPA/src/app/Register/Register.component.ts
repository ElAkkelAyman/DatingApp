import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {
 @Input() valuesFromHome: any;
 @Output() cancelRegister = new EventEmitter();
model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  Register(){
    this.authService.Register(this.model).subscribe(next => {
      this.alertify.success('successfuly registered') ;
      console.log('successfuly registered') ;
     }, error => {
      this.alertify.error(error);
      console.log(error) ;
     });
   
  }
  Cancel() {
    this.cancelRegister.emit();
    this.alertify.message('cancel register !');
  }

}
