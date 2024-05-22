export class Contact {
  id: any = undefined;
  companyId: string = "";
  document: string = "";
  docType: string = "0:SIN DEFINIR";
  name: string = "";
  address: string = "";
  phoneNumber: string = "";
  codUbigeo: string = "-";
}

export class ContactDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public contact: Contact = new Contact()) {
  }
}

export interface IContribuyente {
  ruc: string;
  dni: string;
  nombre: string;
  estado: string;
  condicion_domicilio: string;
  ubigeo: string;
  tipo_via: string;
  nombre_via: string;
  codigo_zona: string;
  tipo_zona: string;
  numero: string;
  interior: string;
  lote: string;
  departamento: string;
  manzana: string;
  kilometro: string;
}
