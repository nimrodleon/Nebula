import {Cuota, PaymentTerms} from "../../cashier/quicksale/interfaces";

export class Cliente {
  tipoDoc: string = "";
  numDoc: string = "";
  rznSocial: string = "";
}

export class BillingResponse {
  success: boolean = false;
  hash: string = "";
  cdrCode: string = "";
  cdrDescription: string = "";
  cdrNotes: string[] = [];
  cdrId: string = "";
}

export class BaseSale {
  id: any = undefined;
  companyId: string = "";
  invoiceSerieId: string = "";
  tipoDoc: string = "03";
  serie: string = "";
  correlativo: string = "";
  fechaEmision: string = "";
  contactId: string = "";
  cliente: Cliente = new Cliente();
  tipoMoneda: string = "PEN";
  mtoOperGravadas: number = 0;
  mtoOperInafectas: number = 0;
  mtoOperExoneradas: number = 0;
  mtoIGV: number = 0;
  totalImpuestos: number = 0;
  valorVenta: number = 0;
  subTotal: number = 0;
  mtoImpVenta: number = 0;
  billingResponse: BillingResponse = new BillingResponse();
  totalEnLetras: string = "";
}

export abstract class SaleDetail {
  id: any = undefined;
  companyId: string = "";
  warehouseId: string = "";
  tipoItem: string = "BIEN";
  unidad: string = "";
  cantidad: number = 0;
  codProducto: string = "";
  description: string = "";
  mtoValorUnitario: number = 0;
  mtoBaseIgv: number = 0;
  porcentajeIgv: number = 0;
  igv: number = 0;
  tipAfeIgv: string = "";
  totalImpuestos: number = 0;
  mtoPrecioUnitario: number = 0;
  mtoValorVenta: number = 0;
  recordType: string = "PRODUCTO";
}

export class InvoiceSale extends BaseSale {
  tipoOperacion: string = "0101";
  fecVencimiento: string = "";
  paymentMethod: string = "";
  formaPago: PaymentTerms = new PaymentTerms();
  cuotas: Cuota[] = [];
  remark: string = "";
  anulada: boolean = false;
}

export class InvoiceSaleDetail extends SaleDetail {
  invoiceSaleId: string = "";
  cajaDiariaId: string = "-";
}

// respuesta comprobante de venta.
export class ResponseInvoiceSale {
  constructor(
    public invoiceSale: InvoiceSale = new InvoiceSale(),
    public invoiceSaleDetails: Array<InvoiceSaleDetail> = new Array<InvoiceSaleDetail>()) {
  }
}

export class ComprobantesPendientes {
  comprobanteId: string = "";
  tipoDoc: string = "";
  serie: string = "";
  correlativo: string = "";
  fechaEmision: string = "";
  mtoImpVenta: number = 0;
  cdrDescription: string = "";
}
