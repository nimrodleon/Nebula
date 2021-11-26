import {ItemNote} from './item-note';

export interface Note {
  contactId: number;
  warehouseId: string;
  noteType: string;
  motivo: number;
  startDate: any;
  remark: string;
  itemNotes: Array<ItemNote>;
}
