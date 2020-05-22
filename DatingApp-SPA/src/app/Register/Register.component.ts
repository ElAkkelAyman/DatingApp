import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';

@Component({
  selector: 'app-register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {
 @Input() valuesFromHome: any;
 @Output() cancelRegister = new EventEmitter();
 registerForm: FormGroup;
model: any = {};
bsConfig: Partial<BsDatepickerConfig>;
  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router,
              private fb: FormBuilder) { }

  ngOnInit() {
    this.bsConfig = {
     containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
    {
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],
      gender: ['male'],
      knownAs: ['', Validators.required],
      DateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator});
  }

  get username() { return this.registerForm.get('username'); }
  get password() { return this.registerForm.get('password'); }
  get confirmPassword() { return this.registerForm.get('confirmPassword'); }
  get gender() { return this.registerForm.get('gender'); }
  get knownAs() { return this.registerForm.get('knownAs'); }
  get DateOfBirth() { return this.registerForm.get('DateOfBirth'); }
  get city() { return this.registerForm.get('city'); }
  get country() { return this.registerForm.get('country'); }

  passwordMatchValidator(g: FormGroup)
  {
    return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch : true };
  }

  Register(){
    if (this.registerForm.valid){
      this.model = Object.assign({}, this.registerForm.value);

      this.authService.Register(this.model).subscribe(next => {
      this.alertify.success('successfuly registered') ;
      console.log('successfuly registered') ;
     }, error => {
      this.alertify.error(error);
      console.log(error) ;
     }, () => {
      this.authService.login(this.model).subscribe(next => {
      this.router.navigate(['/members']);
      });


     });
    }
    console.log(this.registerForm.value);

  }
  Cancel() {
    this.cancelRegister.emit();
    this.alertify.message('cancel register !');
  }

}
