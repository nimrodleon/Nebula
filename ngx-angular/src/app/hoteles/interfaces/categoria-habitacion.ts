import { FormType } from "app/common/interfaces";

export class CategoriaHabitacion {
  id: any = undefined;
  companyId: string = "";
  nombre: string = "";
  estado: string = "ACTIVO";
}

export class CategoriaHabitacionDataModal {
  title: string = "";
  type: FormType = FormType.ADD;
  categoriaHabitacion: CategoriaHabitacion = new CategoriaHabitacion();
}
