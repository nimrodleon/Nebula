import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Category} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private appURL: string = environment.applicationUrl + 'Category';

  constructor(private http: HttpClient) {
  }

  public store(data: Category): Observable<ResponseData<Category>> {
    return this.http.post<ResponseData<Category>>(`${this.appURL}/Store`, data);
  }
}
