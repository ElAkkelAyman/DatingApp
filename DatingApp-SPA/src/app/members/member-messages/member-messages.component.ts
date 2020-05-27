import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: any[];
  newMessage: any = {};

  constructor(private autService: AuthService , private userserv: UserService,
              private alertifyServ: AlertifyService) { }

  ngOnInit() {
    this.LoadMessages();
  }

LoadMessages()
{
  const currentUserId = +this.autService.decodedToken.nameid;
  console.log('Load Messages');
  this.userserv.getMessageThread(this.autService.decodedToken.nameid, this.recipientId)
  .pipe(
   tap(messages => {
     // tslint:disable-next-line: prefer-for-of
     debugger;
     for (let i = 0 ; i < messages.length; i++) {
     if (messages[i].isRead === false && messages[i].recipientId == currentUserId)
     {
        this.userserv.markAsRead(currentUserId, messages[i].id);
     }
     }
   })
  )
  .subscribe(messages => {
   this.messages = messages;

  }, error => {
   this.alertifyServ.error(error);
  });
}
sendMessage()
{
  this.newMessage.recipientId =  this.recipientId;
  this.userserv.sendMessage(this.autService.decodedToken.nameid, this.newMessage).subscribe(message => {
   this.messages.unshift(message);
   this.newMessage.conetent = '';
   this.LoadMessages();
  }, error => {
   this.alertifyServ.error(error);
  });
  
}

}
