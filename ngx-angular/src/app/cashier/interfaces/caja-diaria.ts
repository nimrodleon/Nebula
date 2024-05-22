import {Company} from "../../account/interfaces";
import {Contact} from "../../contact/interfaces";

export class CajaDiaria {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public invoiceSerieId: string = "", // Id Serie de facturación.
    public terminal: string = "", // identificador y nombre - Serie de facturación.
    public status: string = "", // Estado de Caja.
    public totalApertura: number = 0, // Monto Apertura.
    public totalCierre: number = 0, // Monto para el dia siguiente.
    public turno: string = "", // turno del trabajador.
    public createdAt: any = undefined,
    public warehouseId: any = undefined,) {
  }
}

export class QuickSaleConfig {
  company: Company = new Company();
  cajaDiaria: CajaDiaria = new CajaDiaria();
  contact: Contact = new Contact();
}

export interface AperturaCaja {
  serieId: string;
  total: number;
  turno: string;
}

export interface CerrarCaja {
  totalCierre: number;
}

export class ResumenCajaDto {
  constructor(
    public yape: number = 0,
    public credito: number = 0,
    public contado: number = 0,
    public deposito: number = 0,
    public salida: number = 0,
    public montoTotal: number = 0) {
  }
}
