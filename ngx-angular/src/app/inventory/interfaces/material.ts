import moment from "moment";

export class Material {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public user: string = "",
    public contactId: string = "",
    public contactName: string = "",
    public employeeId: string = "",
    public employeeName: string = "",
    public status: "BORRADOR" | "VALIDADO" = "BORRADOR",
    public remark: string = "",
    public createdAt: string = moment().format("YYYY-MM-DD")) {
  }
}

export class MaterialDetail {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public materialId: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public cantSalida: number = 0,
    public productId: string = "",
    public productName: string = "",
    public cantRetorno: number = 0,
    public cantUsado: number = 0,
    public createdAt: string = moment().format("YYYY-MM-DD")) {
  }
}

export class MaterialDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public materialDetail: MaterialDetail = new MaterialDetail()) {
  }
}

export class MaterialDto {
  material: Material = new Material();
  materialDetails: MaterialDetail[] = [];
}
