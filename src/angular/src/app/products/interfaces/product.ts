export class Product {
  id: any = undefined;
  companyId: string = "";
  description: string = "";
  category: string = "";
  barcode: string = "-";
  igvSunat: string = "GRAVADO";
  precioVentaUnitario: number = 0;
  type: string = "";
  undMedida: string = "";
}

export class ProductDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public product: Product = new Product()) {
  }
}
