import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';

@Injectable()

export class MemberDetailResolver implements Resolve<any> {
  constructor(private userService: UserService, private router: Router,
              private alertify: AlertifyService) {}

   resolve(route: ActivatedRouteSnapshot): Observable<any> { // getting data from route before loading the component
       return this.userService.getUser(route.params['id']).pipe(
        catchError(error => {
         this.alertify.error('problem retreiving data');
         this.router.navigate(['/members']);
         return of(null);
        })
       );
   }
}
