import {InvoiceDetail} from './invoice-detail';

export class Invoice {
  id: null | undefined;
  typeDoc: string;
  serie: string;
  number: string;
  tipOperacion: string;
  fecEmision: string;
  horEmision: string;
  fecVencimiento: string;
  codLocalEmisor: string;
  tipDocUsuario: string;
  numDocUsuario: string;
  rznSocialUsuario: string;
  tipMoneda: string;
  sumTotTributos: number;
  sumTotValVenta: number;
  sumPrecioVenta: number;
  sumDescTotal: number;
  sumOtrosCargos: number;
  sumTotalAnticipos: number;
  sumImpVenta: number;
  invoiceDetails: InvoiceDetail | any;

  constructor() {
    this.id = null;
    this.typeDoc = '';
    this.serie = '';
    this.number = '';
    this.tipOperacion = '';
    this.fecEmision = '';
    this.horEmision = '';
    this.fecVencimiento = '';
    this.codLocalEmisor = '';
    this.tipDocUsuario = '';
    this.numDocUsuario = '';
    this.rznSocialUsuario = '';
    this.tipMoneda = '';
    this.sumTotTributos = 0;
    this.sumTotValVenta = 0;
    this.sumPrecioVenta = 0;
    this.sumDescTotal = 0;
    this.sumOtrosCargos = 0;
    this.sumTotalAnticipos = 0;
    this.sumImpVenta = 0;
  }
}
