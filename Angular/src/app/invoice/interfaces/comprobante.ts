import {DetailComprobante} from './detail-comprobante';

export interface Comprobante {
  contactId: number | null;
  startDate: any;
  docType: string;
  cajaId: string;
  paymentType: string;
  typeOperation: string;
  serie: string;
  number: string;
  endDate: any;
  sumTotValVenta: number;
  sumTotTributos: number;
  icbper: number;
  sumImpVenta: number;
  remark: string;
  invoiceType: string;
  details: Array<DetailComprobante>;
}
