import {InvoiceSaleDetail} from './invoice-sale-detail';
import {TributoSale} from './tributoSale';
import {InvoiceSaleAccount} from './invoice-sale-account';
import {InvoiceSale} from './invoiceSale';

// respuesta del servidor
// comprobante de venta detallado.
export interface ResponseInvoiceSale {
  invoiceSale: InvoiceSale;
  invoiceSaleDetails: Array<InvoiceSaleDetail>;
  tributoSales: Array<TributoSale>;
  invoiceSaleAccounts: Array<InvoiceSaleAccount>;
}
