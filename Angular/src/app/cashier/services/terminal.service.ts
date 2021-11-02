import {Injectable} from '@angular/core';
import {ProductService} from '../../products/services';
import {Sale, SaleDetail} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class TerminalService {
  private _sale: Sale = new Sale();

  constructor(
    private productService: ProductService) {
  }

  public get sale(): Sale {
    return this._sale;
  }

  public addItem(prodId: any): void {
    this.productService.show(prodId).subscribe(result => {
      if (this._sale.details.length <= 0) {
        this.sale.details.push(new SaleDetail(result.id, result.description,
          result.price, 1, 0, result.price));
      } else {
        let changeQuantity: boolean = false;
        this._sale.details.forEach(item => {
          if (item.productId === result.id) {
            item.price = result.price;
            item.quantity = item.quantity + 1;
            item.amount = item.quantity * result.price;
            changeQuantity = true;
          }
        });
        if (!changeQuantity) {
          this.sale.details.push(new SaleDetail(result.id, result.description,
            result.price, 1, 0, result.price));
        }
      }
    });
  }

  public changeQuantity(prodId: any, value: number): void {
    this._sale.details.forEach(item => {
      if (item.productId === prodId) {
        item.quantity = value;
        item.amount = item.quantity * item.price;
      }
    });
  }

  public deleteItem(prodId: any): void {
    console.log(prodId);
    this._sale.details.forEach((value, index, array) => {
      if (value.productId === prodId) {
        array.splice(index, 1);
      }
    });
  }

}
