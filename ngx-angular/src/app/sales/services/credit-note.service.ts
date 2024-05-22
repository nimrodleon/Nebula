import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "environments/environment";
import {CreditNote, PrintCreditNoteDto} from "../interfaces";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class CreditNoteService {
  private controller: string = "CreditNote";
  private baseUrl: string = environment.applicationUrl + "sales";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public show(invoiceSaleId: string): Observable<CreditNote> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${invoiceSaleId}`;
    return this.http.get<CreditNote>(url);
  }

  public getPrintCreditNoteDto(id: string): Observable<PrintCreditNoteDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Print/${id}`;
    return this.http.get<PrintCreditNoteDto>(url);
  }

  public reenviar(creditNoteId: string): Observable<any> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Reenviar/${creditNoteId}`;
    return this.http.patch<any>(url, {});
  }

}
