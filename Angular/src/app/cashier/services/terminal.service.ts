import {Injectable} from '@angular/core';
import {ProductService} from '../../products/services';
import {Cuota, Sale, SaleDetail} from '../interfaces';
import {InvoiceService} from '../../invoice/services';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class TerminalService {
  private _sale: Sale = new Sale();

  constructor(
    private productService: ProductService,
    private invoiceService: InvoiceService,) {
  }

  public get sale(): Sale {
    return this._sale;
  }

  public set sale(value: Sale) {
    this._sale = value;
  }

  public deleteSale(): void {
    this._sale = new Sale();
  }

  public setClientId(id: number): void {
    this._sale.clientId = id;
  }

  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      if (this._sale.details.length <= 0) {
        this.sale.details.push(new SaleDetail(result.id, result.description,
          result.price, 1, 0, result.price));
        this.calcImporteVenta();
      } else {
        let changeQuantity: boolean = false;
        this._sale.details.forEach(item => {
          if (item.productId === result.id) {
            item.price = result.price;
            item.quantity = item.quantity + 1;
            item.amount = item.quantity * result.price;
            this.calcImporteVenta();
            changeQuantity = true;
          }
        });
        if (!changeQuantity) {
          this.sale.details.push(new SaleDetail(result.id, result.description,
            result.price, 1, 0, result.price));
          this.calcImporteVenta();
        }
      }
    });
  }

  public changeQuantity(prodId: any, value: number): void {
    this._sale.details.forEach(item => {
      if (item.productId === prodId) {
        item.quantity = value;
        item.amount = item.quantity * item.price;
        this.calcImporteVenta();
      }
    });
  }

  public calcDiscount(prodId: any, value: number): void {
    this._sale.details.forEach(item => {
      if (item.productId === prodId) {
        item.discount = value;
        const amount = item.quantity * item.price;
        item.amount = amount - ((item.discount / 100) * amount);
        this.calcImporteVenta();
      }
    });
  }

  public deleteItem(prodId: any): void {
    this._sale.details.forEach((value, index, array) => {
      if (value.productId === prodId) {
        array.splice(index, 1);
        this.calcImporteVenta();
      }
    });
  }

  public calcImporteVenta(): void {
    let total = 0;
    this._sale.details.forEach(item => {
      total = total + item.amount;
    });
    const precioConIgv: boolean = false;
    if (precioConIgv) {
      this._sale.sumImpVenta = total;
      this._sale.sumTotValVenta = total / 1.18;
      this._sale.sumTotTributos = total - this._sale.sumTotValVenta;
    } else {
      this._sale.sumTotValVenta = total;
      this._sale.sumTotTributos = total * 0.18;
      this._sale.sumImpVenta = this._sale.sumTotValVenta + this._sale.sumTotTributos;
    }
  }

  public addInfo(data: any): void {
    this._sale = {...this._sale, ...data};
  }

  public addCuotas(cuotas: Array<Cuota>): void {
    this._sale.cuotas = cuotas;
  }

  public saveChanges(id: number): Observable<ResponseData<Sale>> {
    if (this._sale.paymentType === 'Contado') this._sale.endDate = '1992-04-05';
    return this.invoiceService.salePos(id, this._sale);
  }


}
