import {ItemNote} from './item-note';

export class Transfer {
  constructor(
    public origin: number = 0, // Id Almacén de origen.
    public target: number = 0, // Id Almacén destino.
    public motivo: number = 0, // Motivo del Traslado.
    public startDate: any = null, // Fecha de registro.
    public itemNotes: Array<ItemNote> = new Array<ItemNote>()) {
  }
}
