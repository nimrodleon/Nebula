import {ItemNote} from './item-note';

export class Note {
  constructor(
    public contactId: number = 0, // Id de Contacto.
    public warehouseId: number = 0, // Id del Almacén.
    public noteType: string = '', // Tipo de Nota (Ingreso|Salida).
    public motivo: number | string = 0, // Motivo de Inventario.
    public startDate: any = null, // Fecha de registro.
    public itemNotes: Array<ItemNote> = new Array<ItemNote>()) {
  }
}
