import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Transfer, TransferFilter, TransferNote} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class TransferNoteService {
  private appURL: string = environment.applicationUrl + 'TransferNote';

  constructor(
    private http: HttpClient) {
  }

  public index(filter: TransferFilter): Observable<TransferNote[]> {
    let params = new HttpParams();
    params = params.append('origin', filter.origin);
    params = params.append('target', filter.target);
    params = params.append('year', filter.year);
    params = params.append('month', filter.month);
    return this.http.get<TransferNote[]>(`${this.appURL}/Index`, {params});
  }

  public show(id: number): Observable<TransferNote> {
    return this.http.get<TransferNote>(`${this.appURL}/Show/${id}`);
  }

  public create(data: Transfer): Observable<ResponseData<Transfer>> {
    return this.http.post<ResponseData<Transfer>>(`${this.appURL}/Create`, data);
  }

  public update(id: number, data: Transfer): Observable<ResponseData<Transfer>> {
    return this.http.put<ResponseData<Transfer>>(`${this.appURL}/Update/${id}`, data);
  }

}
