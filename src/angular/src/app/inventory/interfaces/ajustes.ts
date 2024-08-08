import moment from "moment";

export class AjusteInventario {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public user: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public locationId: string = "",
    public locationName: string = "",
    public status: "BORRADOR" | "VALIDADO" = "BORRADOR",
    public remark: string = "",
    public createdAt: string = moment().format("YYYY-MM-DD")) {
  }
}

export class AjusteInventarioDetail {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public ajusteInventarioId: string = "",
    public productId: string = "",
    public productName: string = "",
    public cantExistente: number = 0,
    public cantContada: number = 0) {
  }
}

export class AjusteInventarioDto {
  ajusteInventario: AjusteInventario = new AjusteInventario();
  ajusteInventarioDetails: AjusteInventarioDetail[] = [];
}
