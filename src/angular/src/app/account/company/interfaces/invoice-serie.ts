export class InvoiceSerie {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public name: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public notaDeVenta: string = "",
    public counterNotaDeVenta: number = 0,
    public boleta: string = "",
    public counterBoleta: number = 0,
    public factura: string = "",
    public counterFactura: number = 0,
    public creditNoteBoleta: string = "",
    public counterCreditNoteBoleta: number = 0,
    public creditNoteFactura: string = "",
    public counterCreditNoteFactura: number = 0) {
  }
}

export class InvoiceSerieDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public invoiceSerie: InvoiceSerie = new InvoiceSerie()) {
  }
}
