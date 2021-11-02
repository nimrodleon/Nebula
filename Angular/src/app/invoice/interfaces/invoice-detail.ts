export class InvoiceDetail {
  id: null | undefined;
  invoiceId: number;
  codUnidadMedida: string;
  ctdUnidadItem: number;
  codProducto: string;
  codProductoSunat: string;
  desItem: string;
  mtoValorUnitario: number;
  sumTotTributosItem: number;
  codTriIgv: string;
  mtoIgvItem: number;
  mtoBaseIgvItem: number;
  nomTributoIgvItem: string;
  codTipTributoIgvItem: string;
  tipAfeIgv: string;
  porIgvItem: string;
  codTriIcbper: string;
  mtoTriIcbperItem: number;
  ctdBolsasTriIcbperItem: number;
  nomTributoIcbperItem: string;
  codTipTributoIcbperItem: string;
  mtoTriIcbperUnidad: number;
  mtoPrecioVentaUnitario: number;
  mtoValorVentaItem: number;
  mtoValorReferencialUnitario: number;

  constructor() {
    this.id = null;
    this.invoiceId = 0;
    this.codUnidadMedida = '';
    this.ctdUnidadItem = 0;
    this.codProducto = '';
    this.codProductoSunat = '';
    this.desItem = '';
    this.mtoValorUnitario = 0;
    this.sumTotTributosItem = 0;
    this.codTriIgv = '';
    this.mtoIgvItem = 0;
    this.mtoBaseIgvItem = 0;
    this.nomTributoIgvItem = '';
    this.codTipTributoIgvItem = '';
    this.tipAfeIgv = '';
    this.porIgvItem = '';
    this.codTriIcbper = '';
    this.mtoTriIcbperItem = 0;
    this.ctdBolsasTriIcbperItem = 0;
    this.nomTributoIcbperItem = '';
    this.codTipTributoIcbperItem = '';
    this.mtoTriIcbperUnidad = 0;
    this.mtoPrecioVentaUnitario = 0;
    this.mtoValorVentaItem = 0;
    this.mtoValorReferencialUnitario = 0;
  }
}
