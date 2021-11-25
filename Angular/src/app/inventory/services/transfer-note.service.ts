import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Transfer, TransferNote} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class TransferNoteService {
  private appURL: string = environment.applicationUrl + 'TransferNote';

  constructor(
    private http: HttpClient) {
  }

  public show(id: number): Observable<TransferNote> {
    return this.http.get<TransferNote>(`${this.appURL}/Show/${id}`);
  }

  public store(data: Transfer): Observable<ResponseData<Transfer>> {
    return this.http.post<ResponseData<Transfer>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: Transfer): Observable<ResponseData<Transfer>> {
    return this.http.put<ResponseData<Transfer>>(`${this.appURL}/Update/${id}`, data);
  }

}
