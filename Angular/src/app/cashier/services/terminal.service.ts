import {Injectable} from '@angular/core';
import {ProductService} from '../../products/services';
import {Cuota, Sale, SaleDetail} from '../interfaces';
import {InvoiceService} from '../../invoice/services';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';
import {CompanyService} from '../../system/services';
import {Company} from '../../system/interfaces';

@Injectable({
  providedIn: 'root'
})
export class TerminalService {
  private _config: Company | any;
  private _sale: Sale = new Sale();

  constructor(
    private productService: ProductService,
    private invoiceService: InvoiceService,
    private companyService: CompanyService) {
  }

  // parámetros del sistema.
  public get config(): any {
    return this._config;
  }

  // información de la venta.
  public get sale(): Sale {
    return this._sale;
  }

  // setter venta.
  public set sale(value: Sale) {
    this._sale = value;
  }

  // borrar venta.
  public deleteSale(): void {
    this._sale = new Sale();
  }

  // setter clienteID.
  public setClientId(id: number): void {
    this._sale.clientId = id;
  }

  // cargar parámetros del sistema.
  public getConfig(): void {
    this.companyService.show()
      .subscribe(result => this._config = result);
  }

  // Agregar item al carrito de compras.
  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      if (this._sale.details.length <= 0) {
        this._sale.details.push(new SaleDetail(
          result.id, result.description, 1, result.price1, result.price1
        ));
        this.calcImporteVenta();
      } else {
        let changeQuantity: boolean = false;
        this._sale.details.forEach((item: SaleDetail) => {
          if (item.productId === result.id) {
            item.quantity = item.quantity + 1;
            item.price = result.price1;
            item.amount = item.quantity * item.price;
            this.calcImporteVenta();
            changeQuantity = true;
          }
        });
        // ejecutar si no hay coincidencias.
        if (!changeQuantity) {
          this._sale.details.push(new SaleDetail(
            result.id, result.description, 1, result.price1, result.price1
          ));
          this.calcImporteVenta();
        }
      }
    });
  }

  // cambiar cantidad item.
  public changeQuantity(prodId: any, value: number): void {
    this._sale.details.forEach((item: SaleDetail) => {
      if (item.productId === prodId) {
        item.quantity = value;
        item.amount = item.quantity * item.price;
        this.calcImporteVenta();
      }
    });
  }

  // borrar item carrito de compras.
  public deleteItem(prodId: any): void {
    this._sale.details.forEach((value: SaleDetail, index: number, array: any) => {
      if (value.productId === prodId) {
        array.splice(index, 1);
        this.calcImporteVenta();
      }
    });
  }

  // calcular importe venta.
  public calcImporteVenta(): void {
    let total = 0;
    const {porcentajeIgv} = this._config;
    this._sale.details.forEach((item: SaleDetail) => {
      total = total + item.amount;
    });
    this._sale.sumTotValVenta = total / ((porcentajeIgv / 100) + 1);
    this._sale.sumTotTributos = total - this._sale.sumTotValVenta;
    this._sale.sumImpVenta = total;
  }

  // agregar información.
  public addInfo(data: any): void {
    this._sale = {...this._sale, ...data};
  }

  // Agregar cuotas.
  public addCuotas(cuotas: Array<Cuota>): void {
    this._sale.cuotas = cuotas;
  }

  // guardar cambios.
  public saveChanges(id: number): Observable<ResponseData<Sale>> {
    if (this._sale.paymentType === 'Contado') this._sale.endDate = '1992-04-05';
    return this.invoiceService.salePos(id, this._sale);
  }

}
