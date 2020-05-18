import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

// const httpOptions = {
//  headers: new HttpHeaders({
//   Authorization: 'Bearer ' + localStorage.getItem('token')
//  })
// };

@Injectable({
  providedIn: 'root'
})
export class UserService {
 baseUrl = environment.apiUrl;


constructor(private http: HttpClient) {

 }
 getUsers(){
  return this.http.get(this.baseUrl + 'users');
 }
 getUser(id: number): Observable<any>{
 return this.http.get<any>(this.baseUrl + 'users/' + id);
 }

 updateUser(id: number, user: any){
  return this.http.put(this.baseUrl + 'users/' + id, user);
 }

}
