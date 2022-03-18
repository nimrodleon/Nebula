import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {ResponseData} from '../../global/interfaces';
import {User, UserRegister} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private appURL: string = environment.applicationUrl + 'User';

  constructor(private http: HttpClient) {
  }

  public index(query: string = ''): Observable<Array<User>> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Array<User>>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<User> {
    return this.http.get<User>(`${this.appURL}/Show/${id}`);
  }

  public create(data: UserRegister): Observable<ResponseData<User>> {
    return this.http.post<ResponseData<User>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: UserRegister): Observable<ResponseData<User>> {
    return this.http.put<ResponseData<User>>(`${this.appURL}/Update/${id}`, data);
  }

  public passwordChange(id: string, passwd: string): Observable<ResponseData<User>> {
    const data = new UserRegister();
    data.password = passwd;
    return this.http.put<ResponseData<User>>(`${this.appURL}/PasswordChange/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<User>> {
    return this.http.delete<ResponseData<User>>(`${this.appURL}/Delete/${id}`);
  }
}
