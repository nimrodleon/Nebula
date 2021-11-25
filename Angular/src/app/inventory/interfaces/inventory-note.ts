import {InventoryNoteDetail} from './inventory-note-detail';

export interface InventoryNote {
  id: number;
  contactId: number;
  warehouseId: string;
  motivo: string;
  startDate: any;
  remark: string;
  status: string;
  inventoryNoteDetails: Array<InventoryNoteDetail>;
}
