import { FormType } from "app/common/interfaces";

export class HabitacionHotel {
  id: any = undefined;
  companyId: string = "";
  nombre: string = "";
  pisoHotelId: string = "";
  categoriaHabitacionId: string = "";
  precio: number = 0;
  tarifaHoras: number = 0;
  estado: string = "DISPONIBLE";
  remark: string = "";
}

export class HabitacionHotelDataModal {
  title: string = "";
  type: FormType = FormType.ADD;
  habitacionHotel: HabitacionHotel = new HabitacionHotel();
}

export class HabitacionDisponible {
  public habitacionId: string = "";
  public nombre: string = "";
  public piso: string = "";
  public categoria: string = "";
  public precio: number = 0;
}
