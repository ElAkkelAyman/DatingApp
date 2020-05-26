import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

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
 getUsers(page? , itemsPerPage?, userParams? , likesParams?): Observable<PaginatedResult<any[]>>{
  const paginatedResult: PaginatedResult<any[]> = new PaginatedResult<any[]>();
  let params = new HttpParams();
  if (page != null && itemsPerPage != null)
  {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);

  }
  if (userParams != null)
  {
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('Gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
  }

  if (likesParams === 'Likers')
  {
    params = params.append('Likers', 'true');
  }
  if (likesParams === 'Likees')
  {
    params = params.append('Likees', 'true');
  }
  return this.http.get<any>(this.baseUrl + 'users', { observe : 'response', params})
  .pipe(
   map(response => {
    paginatedResult.result = response.body.userstoReturn;
    if (response.headers.get('Pagination') != null )
    {
      console.log(JSON.parse(response.headers.get('Pagination')));
    }
    paginatedResult.pagination = response.body.paginationBody;

    return paginatedResult;
   })
  ); }
 getUser(id: number): Observable<any>{
 return this.http.get<any>(this.baseUrl + 'users/' + id);
 }

 updateUser(id: number, user: any){
  return this.http.put(this.baseUrl + 'users/' + id, user);
 }

 setMainPhoto(userId: number, id: number)
 {
  return this.http.post(this.baseUrl + 'user/' + userId + '/photos/' + id + '/setMain', {}); // user/14/photos/18/setMain
}
DeletePhoto(userId: number, id: number)
 {
  return this.http.delete(this.baseUrl + 'user/' + userId + '/photos/' + id + '/DeletePicture', {}); // user/14/photos/18/DeletePicture ex
 }
SendLike(userId: number , recipientId: number)
 {
  return this.http.post(this.baseUrl + 'users/' + userId + '/like/' + recipientId,  {}); // http://localhost:5000/api/users/2/like/9
 }

}
