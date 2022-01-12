import {InvoiceNoteDetail} from './invoice-note-detail';

export class InvoiceNote {
  constructor(
    public id: number | any = null,
    public invoiceId: number | any = null, //Identificador Comprobante.
    public docType: string = '', // Tipo doc. NOTA: (CRÉDITO/DÉBITO) => (NC/ND).
    public invoiceType: string = '', // Tipo factura (Compra|Venta)
    public serie: string = '', // Serie Nota.
    public number: string = '', // Número Nota.
    public tipOperacion: string = '', // Tipo de operación. Catálogo: 51, n2
    public fecEmision: string = '', // Fecha de emisión. Formato: YYYY-MM-DD, an..10
    public horEmision: string = '', // Hora de Emisión. Formato: HH:MM:SS, an..14
    public codLocalEmisor: string = '', // Código del domicilio fiscal o de local anexo del emisor. n3
    public tipDocUsuario: string = '', // Tipo de documento de identidad del adquirente o usuario. Catálogo: 6, an1
    public numDocUsuario: string = '', // Número de documento de identidad del adquirente o usuario. an..15
    public rznSocialUsuario: string = '', // Apellidos y nombres, denominación o razón social del adquirente o usuario. an..100
    public tipMoneda: string = '', // Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
    public codMotivo: string = '', // Código del tipo de Nota  electrónica. Catálogo: 10, an2
    public desMotivo: string = '', //  Descripción de motivo o sustento. an..250
    public tipDocAfectado: string = '', // Tipo de documento del documento que modifica. 01 o 03 o 12, an2
    public numDocAfectado: string = '', // Serie y número del documento que modifica. Formato: XXXX-99999999, n..13
    public sumTotTributos: number = 0, // Sumatoria Tributos. Formato: n(12,2), an..15
    public sumTotValVenta: number = 0, // Total valor de venta. Formato: n(12,2), an..15
    public sumPrecioVenta: number = 0, // Total Precio de Venta. Formato: n(12,2), an..15
    public sumImpVenta: number = 0, // Importe total de la venta, cesión en uso o del servicio prestado. Formato: n(12,2), an..15
    public invoiceNoteDetails: Array<InvoiceNoteDetail> = new Array<InvoiceNoteDetail>()) {
  }
}
