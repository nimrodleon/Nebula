import moment from "moment";
import {ItemComprobanteDto} from "../../cashier/quicksale/interfaces";

export enum FormaPagoComprobante {
  Yape = "Contado:Yape",
  Credito = "Credito:Crédito",
  Contado = "Contado:Contado",
  Deposito = "Contado:Depósito"
}

export class ItemComprobanteModal {
  public typeOper: "ADD" | "EDIT" = "ADD";
  public itemComprobanteDto: ItemComprobanteDto = new ItemComprobanteDto();
}

export class DatoPagoDto {
  constructor(
    public formaPago: string = "",
    public mtoNetoPendientePago: number = 0,) {
  }
}

export class CuotaPagoDto {
  constructor(
    public uuid: string = "-",
    public mtoCuotaPago: number = 0,
    public fecCuotaPago: string = moment().format("YYYY-MM-DD")) {
  }
}

export class CuotaDataModal {
  constructor(
    public type: "ADD" | "EDIT" = "ADD",
    public cuotaPagoDto: CuotaPagoDto = new CuotaPagoDto()) {
  }
}

export class CreditInformationDto {
  constructor(
    public datoPagoDto: DatoPagoDto = new DatoPagoDto(),
    public cuotasPagoDto: Array<CuotaPagoDto> = new Array<CuotaPagoDto>(),
    public fecVencimiento: string = "-") {
  }
}
