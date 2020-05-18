import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable()

export class MemberEditResolver implements Resolve<any> {
  constructor(private userService: UserService, private router: Router,
              private alertify: AlertifyService, private authserv: AuthService) {}

   resolve(route: ActivatedRouteSnapshot): Observable<any> { // getting data from route before loading the component
       return this.userService.getUser(this.authserv.decodedToken.nameid).pipe(
        catchError(error => {
         this.alertify.error('problem retreiving data');
         this.router.navigate(['/home']);
         return of(null);
        })
       );
   }
}
