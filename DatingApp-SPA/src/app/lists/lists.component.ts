import { Component, OnInit } from '@angular/core';
import { PaginatedResult } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  users: any[];
  pagination: any;
  pageNumber = 1;
  pageSize = 5 ;
  likesParam = 'Likers';
  user: any = JSON.parse(localStorage.getItem('user'));

  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  userParams: any = {};  

  constructor(private userserv: UserService, private authService: AuthService, private alertifyserv: AlertifyService) { }

  ngOnInit() {
    this.loadUsers();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page ;
    this.loadUsers();
   }

   loadUsers()
  {
    console.log(this.userParams);
    this.userserv.getUsers(this.pageNumber, this.pageSize , null, this.likesParam)
    .subscribe((users: PaginatedResult<any[]>) => {
     this.users = users.result;
     this.pagination = users.pagination;
    }, error => {
      this.alertifyserv.error(error);
    });
  }

}
