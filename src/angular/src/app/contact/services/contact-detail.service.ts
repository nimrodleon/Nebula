import {Injectable} from "@angular/core";
import {Contact} from "../interfaces";
import {InvoiceSale} from "../../sales/interfaces";
import {ContactService} from "./contact.service";
import {CashierDetail} from "../../cashier/interfaces";
import {Material} from "../../inventory/interfaces";
import {MaterialDetailService, MaterialService} from "app/inventory/services";
import {UserDataService} from "../../common/user-data.service";
import {deleteConfirm, deleteError} from "app/common/interfaces";
import {CuentaPorCobrarClienteAnualParam, Receivable, ReceivableRequestParams} from "../../receivable/interfaces";
import {ReceivableService} from "../../receivable/services";
import moment from "moment";
import _ from "lodash";

@Injectable({
  providedIn: "root"
})
export class ContactDetailService {
  private _contact: Contact = new Contact();
  private _registroDeVentas: Array<InvoiceSale> = new Array<InvoiceSale>();
  private _entradaSalida: Array<CashierDetail> = new Array<CashierDetail>();
  private _materials: Array<Material> = new Array<Material>();
  private _cuentasPorCobrar: Array<Receivable> = new Array<Receivable>();

  constructor(
    private userDataService: UserDataService,
    private contactService: ContactService,
    private materialService: MaterialService,
    private materialDetailService: MaterialDetailService,
    private receivableService: ReceivableService) {
  }

  public reset(): void {
    this._contact = new Contact();
    this._registroDeVentas = new Array<InvoiceSale>();
    this._entradaSalida = new Array<CashierDetail>();
    this._materials = new Array<Material>();
  }

  public getDetail(contactId: string, tab: string): void {
    const year: string = moment().format("YYYY");
    const month: string = moment().format("MM");
    this.contactService.show(contactId).subscribe(result => {
      this._contact = result;
      if (tab === "ventas") {
        this.getRegistroDeVentas(contactId, month, year);
      }
      if (tab === "dinero") {
        this.getEntradaSalidaDeDinero(month, year);
      }
      if (tab === "materiales") {
        this.getMaterials(month, year);
      }
      if (tab === "receivable") {
        const requestParam = new CuentaPorCobrarClienteAnualParam();
        requestParam.year = year;
        requestParam.status = "PENDIENTE";
        this.getCuentasPorCobrar(requestParam);
      }
    });
  }

  public get contact(): Contact {
    return this._contact;
  }

  public getContact(contactId: string): void {
    this.contactService.show(contactId)
      .subscribe(result => this._contact = result);
  }

  public get registroDeVentas(): Array<InvoiceSale> {
    return this._registroDeVentas;
  }

  public getRegistroDeVentas(contactId: string, month: string, year: string): void {
    this.contactService.invoiceSale(contactId, month, year)
      .subscribe(result => this._registroDeVentas = result);
  }

  public get entradaSalida(): Array<CashierDetail> {
    return this._entradaSalida;
  }

  public get cuentasPorCobrar(): Array<Receivable> {
    return this._cuentasPorCobrar;
  }

  public getEntradaSalidaDeDinero(month: string, year: string): void {
    this.contactService.entradaSalida(this._contact.id, month, year)
      .subscribe(result => this._entradaSalida = result);
  }

  public get materials(): Array<Material> {
    return this._materials;
  }

  public getMaterials(month: string, year: string): void {
    this.materialService.getByContactId(year, month, this._contact.id)
      .subscribe(result => this._materials = result);
  }

  public getCuentasPorCobrar(param: CuentaPorCobrarClienteAnualParam): void {
    param.contactId = this._contact.id;
    this.receivableService.getCargosCliente(param)
      .subscribe(result => this._cuentasPorCobrar = result);
  }

  public deleteMaterial(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.materialDetailService.countDocuments(id)
          .subscribe(totalDocuments => {
            if (totalDocuments > 0) {
              deleteError().then(() => console.log(totalDocuments));
            } else {
              this.materialService.delete(id).subscribe(result => {
                this._materials = _.filter(this._materials, (o: Material) => o.id !== result.id);
              });
            }
          });
      }
    });
  }

}
