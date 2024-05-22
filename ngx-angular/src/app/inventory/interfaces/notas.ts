import moment from "moment";

export class InventoryNotas {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public user: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public contactId: string = "",
    public contactName: string = "",
    public transactionType: "ENTRADA" | "SALIDA" = "ENTRADA",
    public status: "BORRADOR" | "VALIDADO" = "BORRADOR",
    public remark: string = "",
    public createdAt: string = moment().format("YYYY-MM-DD")) {
  }
}

export class InventoryNotasDetail {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public inventoryNotasId: string = "",
    public productId: string = "",
    public productName: string = "",
    public demanda: number = 0) {
  }
}

export class InventoryProduct {
  constructor(
    public id: any = undefined,
    public productId: string = "",
    public productName: string = "",
    public cantidad: number = 0) {
  }
}

export class InventoryProductDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public inventoryProduct: InventoryProduct = new InventoryProduct()) {
  }
}

export class InventoryNoteDto {
  inventoryNotas: InventoryNotas = new InventoryNotas();
  inventoryNotasDetail: InventoryNotasDetail[] = [];
}
