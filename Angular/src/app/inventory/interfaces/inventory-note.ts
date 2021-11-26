import {InventoryNoteDetail} from './inventory-note-detail';
import {Contact} from '../../contact/interfaces';
import {Warehouse} from './warehouse';

export interface InventoryNote {
  id: number;
  contact: Contact | any;
  contactId: number;
  warehouse: Warehouse | any;
  warehouseId: string;
  motivo: string;
  startDate: any;
  remark: string;
  status: string;
  inventoryNoteDetails: Array<InventoryNoteDetail>;
}
