import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable()

export class MessagesResolver implements Resolve<any> {
pageNumber = 1;
pageSize = 5;
messageContainer = 'Unread';
  constructor(private userService: UserService, private router: Router,
              private alertify: AlertifyService, private authServ: AuthService) {}

   resolve(route: ActivatedRouteSnapshot): Observable<any> { // getting data from route before loading the component
       return this.userService.getMessages(this.authServ.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer).pipe(
        catchError(error => {
         this.alertify.error('problem retreiving Messages');
         this.router.navigate(['/home']);
         return of(null);
        })
       );
   }
}
