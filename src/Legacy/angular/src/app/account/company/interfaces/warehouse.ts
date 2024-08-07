export class Warehouse {
  constructor(
    public id: string | any = undefined,
    public companyId: string = "",
    public name: string = "",
    public remark: string = "") {
  }
}

export class WarehouseDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public warehouse: Warehouse = new Warehouse()) {
  }
}
