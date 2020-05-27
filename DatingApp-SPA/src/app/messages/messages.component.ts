import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
messages: any[];
pagination: any;
messageContainer = 'Unread';
  constructor(private userService: UserService , private authservice: AuthService
    ,         private rout: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
   this.rout.data.subscribe(
   data => {
   this.messages = data.messages.result;
   this.pagination = data.messages.pagination;
   });
  }

  loadMessages()
  {
    this.userService.getMessages(this.authservice.decodedToken.nameid, this.pagination.currentPage,
      this.pagination.itemsPerPage, this.messageContainer)
      .subscribe((res: PaginatedResult<any[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
       }, error => {
         this.alertify.error(error);
       });
  }

  pageChanged(event: any) : void {
   this.pagination.currentPage = event.page;
   this.loadMessages();
  }

  DeleteMessage(id: number){
  this.alertify.confirm('Are you sure you want to delete this message ?', () => {
    this.userService.deleteMessage(this.authservice.decodedToken.nameid, id).subscribe(() => {
     this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
     this.alertify.success('Message has been deleted');
    }, error => {
      this.alertify.error(error);
    });
  });


  }

}
