import {FormType} from "app/common/interfaces";
import {Company} from "../../account/interfaces";

export type RepairOrderStatus = "PENDIENTE" | "EN PROCESO" | "FINALIZADO" | "ENTREGADO" | "ARCHIVADO";

export enum RepairOrderEnumStatus {
  PENDIENTE = "PENDIENTE",
  EN_PROCESO = "EN PROCESO",
  FINALIZADO = "FINALIZADO",
  ENTREGADO = "ENTREGADO",
  ARCHIVADO = "ARCHIVADO"
}

export class RepairOrder {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public serie: string = "",
    public number: string = "",
    public idCliente: string = "",
    public nombreCliente: string = "",
    public datosEquipo: string = "",
    public tareaRealizar: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public technicalId: string = "",
    public technicalName: string = "",
    public status: RepairOrderStatus = RepairOrderEnumStatus.PENDIENTE,
    public invoiceSerieId: string = "",
    public repairAmount: number = 0,
    public createdAt: any = undefined,
    public updatedAt: any = undefined) {
  }
}

export class ItemRepairOrder {
  constructor(
    public id: any = undefined,
    public repairOrderId: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public quantity: number = 0,
    public precioUnitario: number = 0,
    public productId: string = "",
    public description: string = "",
    public monto: number = 0) {
  }
}

export class ItemRepairOrderDataModal {
  constructor(
    public title: string = "",
    public type: string = FormType.ADD,
    public itemRepairOrder: ItemRepairOrder = new ItemRepairOrder()) {
  }
}

export class TallerRepairOrderTicket {
  constructor(
    public company: Company = new Company(),
    public repairOrder: RepairOrder = new RepairOrder(),
    public itemsRepairOrder: Array<ItemRepairOrder> = new Array<ItemRepairOrder>()) {
  }
}
