import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: any[];
  user: any = JSON.parse(localStorage.getItem('user'));
  Pagination: any;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  userParams: any = {};

  constructor(private userserv: UserService, private alertify: AlertifyService) { }

  ngOnInit() {     
    this.userParams.gender =  this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
    this.LoadUsers();
    
  }
  pageChanged(event: any): void {
   this.Pagination.currentPage = event.page ;
   this.LoadUsers();
  }

  ResetFilter() 
  {
    this.userParams.gender =  this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
    this.LoadUsers();
  }

  LoadUsers()
  {
    console.log(this.userParams);
    this.userserv.getUsers(this.Pagination?.currentPage, this.Pagination?.itemsPerPage, this.userParams)
    .subscribe((users: PaginatedResult<any[]>) => {
     this.users = users.result;
     this.Pagination = users.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
