import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ProductService} from '../../products/services';
import {Sale} from '../interfaces';
import {InvoiceService} from '../../invoice/services';
import {ResponseData} from '../../global/interfaces';
import {Configuration} from '../../system/interfaces';
import {Product} from '../../products/interfaces';
import {CpeDetail} from '../../invoice/interfaces';

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

  // establecer el ID del contacto.
  public setContactId(id: number): void {
    this._sale.contactId = id;
  }

  // cargar parámetros del sistema.
  public setConfig(value: any): void {
    this._config = value;
  }

  // configurar detalle venta.
  private getDetalleVenta(data: Product): CpeDetail {
    const detalleVenta: CpeDetail = new CpeDetail();
    detalleVenta.productId = data.id;
    detalleVenta.codUnidadMedida = data.undMedida.sunatCode;
    detalleVenta.codProductoSunat = data.barcode.length > 0 ? data.barcode : '-';
    detalleVenta.description = data.description;
    detalleVenta.price = data.price1;
    detalleVenta.quantity = 1;
    detalleVenta.igvSunat = data.igvSunat;
    detalleVenta.valorIgv = this._config.porcentajeIgv;
    detalleVenta.triIcbper = data.icbper === 'SI';
    detalleVenta.valorIcbper = this._config.valorImpuestoBolsa;
    detalleVenta.porcentajeIGV = this._config.porcentajeIgv;
    detalleVenta.mtoTriIcbperUnidad = this._config.valorImpuestoBolsa;
    detalleVenta.calcularItem();
    return detalleVenta;
  }

  // Agregar item al carrito de compras.
  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      if (this._sale.details.length <= 0) {
        this._sale.details.push(this.getDetalleVenta(result));
        this._sale.calcImporteVenta();
      } else {
        let changeQuantity: boolean = false;
        this._sale.details.forEach((item: CpeDetail) => {
          if (item.productId === result.id) {
            item.quantity = item.quantity + 1;
            item.price = result.price1;
            item.calcularItem();
            this._sale.calcImporteVenta();
            changeQuantity = true;
          }
        });
        // ejecutar si no hay coincidencias.
        if (!changeQuantity) {
          this._sale.details.push(this.getDetalleVenta(result));
          this._sale.calcImporteVenta();
        }
      }
    });
  }

  // cambiar cantidad item.
  public changeQuantity(prodId: any, value: number): void {
    this._sale.details.forEach((item: CpeDetail) => {
      if (item.productId === prodId) {
        item.quantity = value;
        item.calcularItem();
        this._sale.calcImporteVenta();
      }
    });
  }

  // borrar item carrito de compras.
  public deleteItem(prodId: any): void {
    this._sale.details.forEach((value: CpeDetail, index: number, array: any) => {
      if (value.productId === prodId) {
        array.splice(index, 1);
        this._sale.calcImporteVenta();
      }
    });
  }

  // agregar información.
  public addInfo(data: any): void {
    this._sale = {...this._sale, ...data};
  }

  // guardar cambios.
  public saveChanges(id: number): Observable<ResponseData<Sale>> {
    return this.invoiceService.createQuickSale(id, this._sale);
  }

}
