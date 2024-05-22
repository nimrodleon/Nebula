import { FormType } from "app/common/interfaces";

export class PisoHotel {
  id: any = undefined;
  companyId: string = "";
  nombre: string = "";
  estado: string = "ACTIVO";
}

export class PisoHotelDataModal {
  title: string = "";
  type: FormType = FormType.ADD;
  pisoHotel: PisoHotel = new PisoHotel();
}
