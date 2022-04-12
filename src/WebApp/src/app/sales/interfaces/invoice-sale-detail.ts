export class InvoiceSaleDetail {
  constructor(
    public id: any = undefined,
    public cajaDiaria: string = '', // clave foránea caja diaria.
    public invoiceSale: string = '', // Clave foránea comprobante de venta.
    public tipoItem: string = '', // Tipo registro producto o servicio.
    public codUnidadMedida: string = '', // Código de unidad de medida por ítem. Catálogo: 3, an..3
    public ctdUnidadItem: number = 0, //  Cantidad de unidades por ítem. an..23|n(12,10)
    public codProducto: string = '', // Código de producto. an..30
    public codProductoSunat: string = '', // Código producto SUNAT. Catálogo: 25, an..8
    public desItem: string = '', // Descripción detallada del servicio prestado, bien vendido o cedido en uso
    public mtoValorUnitario: number = 0, // Valor Unitario (cac:InvoiceLine/cac:Price/cbc:PriceAmount). an..23|n(12,10)
    public sumTotTributosItem: number = 0, // Sumatoria Tributos por item. an..15|n(12,2)
    public codTriIgv: string = '', // Tributo: Códigos de tipos de tributos IGV. Catálogo: 5, n4
    public mtoIgvItem: number = 0, // Tributo: Monto de IGV por ítem. an..15|n(12,2)
    public mtoBaseIgvItem: number = 0, // Tributo: Base Imponible IGV por Item. n(12,2)
    public nomTributoIgvItem: string = '', // Tributo: Nombre de tributo por item. an..4 Catálogo: 5,name
    public codTipTributoIgvItem: string = '', // Tributo: Código de tipo de tributo por Item. Catálogo: 5 an4
    public tipAfeIgv: string = '', // Tributo: Afectación al IGV por ítem. Catálogo: 7 an2
    public porIgvItem: string = '', // Tributo: Porcentaje de IGV. an..5 =18.0
    public codTriIcbper: string = '', // Tributo ICBPER: Códigos de tipos de tributos ICBPER. Catálogo: 5 n4
    public mtoTriIcbperItem: number = 0, // Tributo ICBPER: Monto de tributo ICBPER por iItem. an..15|n(12,2)
    public ctdBolsasTriIcbperItem: number = 0, // Tributo ICBPER: Cantidad de bolsas plásticas por Item. n(5)
    public nomTributoIcbperItem: string = '', // Tributo ICBPER:  Nombre de tributo ICBPER por item. Catálogo: 5 an..4
    public codTipTributoIcbperItem: string = '', // Tributo ICBPER: Código de tipo de tributo ICBPER por Item. Catálogo: 5 an4
    public mtoTriIcbperUnidad: number = 0, // Tributo ICBPER: Monto de tributo ICBPER por Unidad. an..5
    public mtoPrecioVentaUnitario: number = 0, // Precio de venta unitario cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice an..23|n(12,10)
    public mtoValorVentaItem: number = 0, // Valor de venta por Item cac:InvoiceLine/cbc:LineExtensionAmount an..15|n(12,2)
    public mtoValorReferencialUnitario: number = 0) {
  }
}
