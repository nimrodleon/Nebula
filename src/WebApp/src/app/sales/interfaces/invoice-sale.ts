export class InvoiceSale {
  constructor(
    public id: any = undefined,
    public docType: string = '', // Tipo documento para control interno. (FT|BL|NV)
    public serie: string = '', // Serie comprobante.
    public number: string = '', // Número comprobante.
    public tipOperacion: string = '', // Tipo de operación Catálogo: 51, n4
    public fecEmision: string = '', // fecha de emisión. Formato: YYYY-MM-DD, an..10
    public horEmision: string = '', // hora emisión. Formato: HH:MM:SS, an..14
    public fecVencimiento: string = '', // fecha de vencimiento. Formato: YYYY-MM-DD, an..10
    public codLocalEmisor: string = '', // Código del domicilio fiscal o de local anexo del emisor.
    public formaPago: string = '', // Forma de pago. Credito / Contado - a7
    public contactId: number | any = undefined, // ID de contacto, Usado para editar el contacto del comprobante.
    public tipDocUsuario: string = '', // Tipo de documento de identidad del adquirente o usuario. Catálogo: 6, an1
    public numDocUsuario: string = '', // Número de documento de identidad del adquirente o usuario. an..15
    public rznSocialUsuario: string = '', // Apellidos y nombres, denominación o razón social del adquirente o usuario. an..1500
    public tipMoneda: string = '', // Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
    public sumTotTributos: number = 0, // Sumatoria Tributos. an..15|n(12,2)
    public sumTotValVenta: number = 0, // Total valor de venta. an..15|n(12,2)
    public sumPrecioVenta: number = 0, // Total Precio de Venta. an..15|n(12,2)
    public sumImpVenta: number = 0) {
  }
}
