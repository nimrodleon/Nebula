import moment from "moment";

export class Transferencia {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public user: string = "",
    public warehouseOriginId: string = "",
    public warehouseOriginName: string = "",
    public warehouseTargetId: string = "",
    public warehouseTargetName: string = "",
    public status: "BORRADOR" | "VALIDADO" = "BORRADOR",
    public remark: string = "",
    public createdAt: string = moment().format("YYYY-MM-DD")) {
  }
}

export class TransferenciaDetail {
  constructor(
    public id: any = undefined,
    public transferenciaId: string = "",
    public productId: string = "",
    public productName: string = "",
    public cantExistente: number = 0,
    public cantTransferido: number = 0,
    public cantRestante: number = 0) {
  }
}

export class TransferenciaDto {
  transferencia: Transferencia = new Transferencia();
  transferenciaDetails: TransferenciaDetail[] = [];
}
