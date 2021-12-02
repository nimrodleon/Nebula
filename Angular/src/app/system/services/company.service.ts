import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Company} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private appURL: string = environment.applicationUrl + 'Company';

  constructor(
    private http: HttpClient) {
  }

  public show(): Observable<Company> {
    return this.http.get<Company>(`${this.appURL}/Show`);
  }

  public update(id: number, data: Company): Observable<ResponseData<Company>> {
    return this.http.put<ResponseData<Company>>(`${this.appURL}/Update/${id}`, data);
  }
}
