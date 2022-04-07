import {InvoiceSale} from './invoice-sale';
import {InvoiceSaleDetail} from './invoice-sale-detail';
import {TributoSale} from './tributo-sale';

// respuesta comprobante de venta.
export class ResponseInvoiceSale {
  constructor(
    public invoiceSale: InvoiceSale = new InvoiceSale(),
    public invoiceSaleDetail: InvoiceSaleDetail = new InvoiceSaleDetail(),
    public tributoSale: TributoSale = new TributoSale()) {
  }
}
