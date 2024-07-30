import {Injectable} from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {environment} from "environments/environment";
import {CreditNote, PrintCreditNoteDto} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class CreditNoteService {
  private controller: string = "CreditNote";
  private baseUrl: string = environment.applicationUrl + "sales";

  constructor(
    private http: HttpClient,) {
  }

  public show(invoiceSaleId: string): Observable<CreditNote> {
    const url: string = `${this.baseUrl}/${this.controller}/${invoiceSaleId}`;
    return this.http.get<CreditNote>(url);
  }

  public getPrintCreditNoteDto(id: string): Observable<PrintCreditNoteDto> {
    const url: string = `${this.baseUrl}/${this.controller}/Print/${id}`;
    return this.http.get<PrintCreditNoteDto>(url);
  }

  public reenviar(creditNoteId: string): Observable<any> {
    const url: string = `${this.baseUrl}/${this.controller}/Reenviar/${creditNoteId}`;
    return this.http.patch<any>(url, {});
  }

}
