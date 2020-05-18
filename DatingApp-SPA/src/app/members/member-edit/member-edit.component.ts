import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  constructor(private route: ActivatedRoute, private Allertif: AlertifyService , private Userservice: UserService) { }
  @ViewChild('editForm', {static: true}) editForm: NgForm;
user: any;
  @HostListener('window:beforeunload',['$event']) // adding this will make sure to show error msg when refreshing or leaving browser
  unloadNotification($event: any){
    if (this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  ngOnInit() {
    this.route.data.subscribe( data => {
      this.user = data.user;
    });
  }

  updateUser(){
   console.log(this.user.id);
   
   this.Userservice.updateUser(this.user.id, this.user).subscribe(next => {
    this.Allertif.success('Profile updated successfuly');
    this.editForm.reset(this.user);
   }, error => {this.Allertif.error(error); }
   );

  }

}
