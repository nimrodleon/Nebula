export class ProductStock {
  id: any = undefined;
  companyId: string = "";
  warehouseId: string = "";
  productId: string = "";
  transactionType: string = "ENTRADA";
  quantity: number = 0;
}

export class ProductStockInfo {
  warehouseId: string = "";
  warehouseName: string = "";
  productId: string = "";
  quantity: number = 0;
}

export class ChangeQuantityStockRequestParams {
  constructor(
    public warehouseId: string = "",
    public productId: string = "",
    public productLoteId: string = "-",
    public quantity: number = 0) {
  }
}
