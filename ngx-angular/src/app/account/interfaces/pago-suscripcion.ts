import { FormType } from "app/common/interfaces";

export class PagoSuscripcion {
  id: any = undefined;
  userId: string = "";
  companyId: string = "";
  companyName: string = "";
  fechaDesde: string = "";
  fechaHasta: string = "";
  monto: number = 0;
  remark: string = "";
  createdAt: string = "";
}

export class PagoSuscripcionDataModal {
  type: FormType = FormType.ADD;
  pagoSuscripcion: PagoSuscripcion = new PagoSuscripcion();
}
