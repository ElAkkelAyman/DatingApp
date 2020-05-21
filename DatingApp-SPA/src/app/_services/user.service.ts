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

 setMainPhoto(userId: number, id: number)
 {
  return this.http.post(this.baseUrl + 'user/' + userId + '/photos/' + id + '/setMain', {});//user/14/photos/18/setMain
}
DeletePhoto(userId: number, id: number)
 {
  return this.http.delete(this.baseUrl + 'user/' + userId + '/photos/' + id + '/DeletePicture', {});//user/14/photos/18/DeletePicture ex
 }
 

}
