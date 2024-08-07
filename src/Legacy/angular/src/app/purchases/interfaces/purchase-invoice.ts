export class PurchaseInvoice {
  constructor(
    public id: any = undefined,
    public fecEmision: string = "",
    public docType: string = "",
    public serie: string = "",
    public number: string = "",
    public fecVencimiento: string = "",
    public formaPago: string = "",
    public contactId: string = "",
    public tipDocProveedor: string = "",
    public numDocProveedor: string = "",
    public rznSocialProveedor: string = "",
    public tipMoneda: string = "PEN",
    public tipoCambio: number = 1,
    public sumTotTributos: number = 0,
    public sumTotValCompra: number = 0,
    public sumPrecioCompra: number = 0,
    public sumImpCompra: number = 0,) {
  }
}

export class PurchaseInvoiceDetail {
  constructor(
    public id: any = undefined,
    public purchaseInvoiceId: string = "",
    public tipoItem: string = "",
    public codUnidadMedida: string = "",
    public ctdUnidadItem: number = 0,
    public codProducto: string = "",
    public desItem: string = "",
    public mtoValorUnitario: number = 0,
    public sumTotTributosItem: number = 0,
    public codTriIgv: string = "",
    public mtoIgvItem: number = 0,
    public mtoBaseIgvItem: number = 0,
    public nomTributoIgvItem: string = "",
    public codTipTributoIgvItem: string = "",
    public tipAfeIgv: string = "",
    public porIgvItem: number = 0,
    public codTriIcbper: string = "",
    public mtoTriIcbperItem: number = 0,
    public ctdBolsasTriIcbperItem: number = 0,
    public nomTributoIcbperItem: string = "",
    public codTipTributoIcbperItem: string = "",
    public mtoTriIcbperUnidad: number = 0,
    public mtoPrecioCompraUnitario: number = 0,
    public mtoValorCompraItem: number = 0,
    public mtoValorReferencialUnitario: number = 0,) {
  }
}

export class PurchaseDto {
  constructor(
    public purchaseInvoice: PurchaseInvoice = new PurchaseInvoice(),
    public purchaseInvoiceDetails: Array<PurchaseInvoiceDetail> = new Array<PurchaseInvoiceDetail>(),
  ) {
  }
}

export class CabeceraCompraDto {
  constructor(
    public docType: string = "",
    public contactId: string = "",
    public tipDocProveedor: string = "",
    public numDocProveedor: string = "",
    public rznSocialProveedor: string = "",
    public fecEmision: string = "",
    public serieComprobante: string = "",
    public numComprobante: string = "",
    public tipoMoneda: string = "PEN",
    public tipoDeCambio: number = 1,) {
  }
}

export class ItemCompraForm {
  constructor(
    public id: any = "-",
    public productId: string = "",
    // #region CONTROL_ITEM_FRONT!
    public typeOper: "ADD" | "EDIT" = "ADD",
    // #endregion
    public tipoItem: string = "BIEN",
    public ctdUnidadItem: number = 0,
    public mtoPrecioCompraUnitario: number = 0,
    public codUnidadMedida: string = "NIU:UNIDAD (BIENES)",
    public desItem: string = "",
    public triIcbper: boolean = false,
    public mtoValorCompraItem: number = 0,
    public igvSunat: string = "GRAVADO",
    public mtoIgvItem: number = 0,
    public mtoTriIcbperItem: number = 0,
    public mtoTotalItem: number = 0,) {
  }
}

export class ItemCompraDto {
  constructor(
    public id: any = undefined,
    public productId: string = "",
    public tipoItem: string = "BIEN",
    public ctdUnidadItem: number = 0,
    public codUnidadMedida: string = "NIU:UNIDAD (BIENES)",
    public desItem: string = "",
    public triIcbper: boolean = false,
    public igvSunat: string = "GRAVADO",
    public mtoPrecioCompraUnitario: number = 0,) {
  }
}
