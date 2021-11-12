import {DetalleComprobante} from './detalle-comprobante';

export class Comprobante {
  contactId: number | null;
  startDate: any;
  docType: string;
  cajaId: string;
  formaPago: string;
  typeOperation: string;
  serie: string;
  numero: string;
  endDate: any;
  sumTotValVenta: number;
  sumTotTributos: number;
  icbper: number;
  sumImpVenta: number;
  remark: string;
  details: Array<DetalleComprobante>;

  constructor() {
    this.contactId = null;
    this.docType = '';
    this.cajaId = '';
    this.formaPago = '';
    this.typeOperation = '';
    this.serie = '';
    this.numero = '';
    this.sumTotValVenta = 0;
    this.sumTotTributos = 0;
    this.icbper = 0;
    this.sumImpVenta = 0;
    this.remark = '';
    this.details = new Array<DetalleComprobante>();
  }
}
