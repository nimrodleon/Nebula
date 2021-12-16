import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ProductService} from '../../products/services';
import {Sale, SaleDetail} from '../interfaces';
import {InvoiceService} from '../../invoice/services';
import {ResponseData} from '../../global/interfaces';
import {Configuration} from '../../system/interfaces';

@Injectable({
  providedIn: 'root'
})
export class TerminalService {
  private _config: Configuration | any;
  private _sale: Sale = new Sale();

  constructor(
    private productService: ProductService,
    private invoiceService: InvoiceService) {
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
  public setConfig(value: any): void {
    this._config = value;
  }

  // Agregar item al carrito de compras.
  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      if (this._sale.details.length <= 0) {
        this._sale.details.push(new SaleDetail(
          result.id, result.description, result.price1, 1, result.price1
        ));
        this.calcImporteVenta();
      } else {
        let changeQuantity: boolean = false;
        this._sale.details.forEach((item: SaleDetail) => {
          if (item.productId === result.id) {
            item.quantity = item.quantity + 1;
            item.price = result.price1;
            item.amount = item.price * item.quantity;
            this.calcImporteVenta();
            changeQuantity = true;
          }
        });
        // ejecutar si no hay coincidencias.
        if (!changeQuantity) {
          this._sale.details.push(new SaleDetail(
            result.id, result.description, result.price1, 1, result.price1
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
        item.amount = item.price * item.quantity;
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
    let sumImpVenta = 0;
    const {porcentajeIgv} = this._config;
    this._sale.details.forEach((item: SaleDetail) => {
      sumImpVenta = sumImpVenta + item.amount;
    });
    this._sale.sumTotValVenta = sumImpVenta / ((porcentajeIgv / 100) + 1);
    this._sale.sumTotTributos = sumImpVenta - this._sale.sumTotValVenta;
    this._sale.sumImpVenta = sumImpVenta;
  }

  // agregar información.
  public addInfo(data: any): void {
    this._sale = {...this._sale, ...data};
  }

  // guardar cambios.
  public saveChanges(id: number): Observable<ResponseData<Sale>> {
    return this.invoiceService.salePos(id, this._sale);
  }

}
