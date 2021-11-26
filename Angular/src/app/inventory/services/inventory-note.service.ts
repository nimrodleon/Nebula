import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {Observable} from 'rxjs';
import {InventoryNote, Note, NoteFilter} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InventoryNoteService {
  private appURL: string = environment.applicationUrl + 'InventoryNote';

  constructor(
    private http: HttpClient) {
  }

  public index(filter: NoteFilter): Observable<InventoryNote[]> {
    let params = new HttpParams();
    params = params.append('warehouseId', filter.warehouseId);
    params = params.append('year', filter.year);
    params = params.append('month', filter.month);
    return this.http.get<InventoryNote[]>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: number): Observable<InventoryNote> {
    return this.http.get<InventoryNote>(`${this.appURL}/Show/${id}`);
  }

  public store(data: Note): Observable<ResponseData<Note>> {
    return this.http.post<ResponseData<Note>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: Note): Observable<ResponseData<Note>> {
    return this.http.put<ResponseData<Note>>(`${this.appURL}/Update/${id}`, data);
  }

}
