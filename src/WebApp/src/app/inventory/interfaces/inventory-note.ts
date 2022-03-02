import {InventoryNoteDetail} from './inventory-note-detail';
import {Contact} from '../../contact/interfaces';
import {Warehouse} from '../../system/interfaces';

export class InventoryNote {
  constructor(
    public id: number | any = null,
    public contact: Contact | any = undefined,
    public contactId: number = 0, // Id de Contacto.
    public warehouse: Warehouse | any = undefined,
    public warehouseId: number = 0, // Id Almacén.
    public noteType: string = '', // Tipo de Nota (Ingreso|Salida).
    public motivo: string = '', // Motivo de Inventario.
    public startDate: any = null, // Fecha de Registro.
    public status: string = '', // Estado de la Nota.
    public inventoryNoteDetails: Array<InventoryNoteDetail> = new Array<InventoryNoteDetail>()) {
  }
}
