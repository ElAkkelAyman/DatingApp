import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() user: any;
  constructor(private userService: UserService, private authService: AuthService, private alertifyserv: AlertifyService) {

  }

  ngOnInit() {
  }



  sendLike(id: number){
   console.log('send like method' + this.user.id);
   this.userService.SendLike(this.authService.decodedToken.nameid, id).subscribe( next => {
    this.alertifyserv.success('you have liked him');
    console.log('you have liked ' + this.user.knownAs);
   }, error => {

    this.alertifyserv.error(error);
   });

}
}
