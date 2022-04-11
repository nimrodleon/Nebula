import {InvoiceSale} from './invoice-sale';
import {InvoiceSaleDetail} from './invoice-sale-detail';
import {TributoSale} from './tributo-sale';

// respuesta comprobante de venta.
export class ResponseInvoiceSale {
  constructor(
    public invoiceSale: InvoiceSale = new InvoiceSale(),
    public invoiceSaleDetails: Array<InvoiceSaleDetail> = new Array<InvoiceSaleDetail>(),
    public tributoSales: Array<TributoSale> = new Array<TributoSale>()) {
  }
}
