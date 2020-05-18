import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: any[];

  constructor(private userserv: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.LoadUsers();
  }
  LoadUsers()
  {
    this.userserv.getUsers().subscribe((users: any[]) => {
     this.users = users;

    }, error => {
      this.alertify.error(error);
    });
  }

}
