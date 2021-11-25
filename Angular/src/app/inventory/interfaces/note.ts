import {ItemNote} from './item-note';

export interface Note {
  contactId: number;
  warehouseId: string;
  motivo: string;
  startDate: any;
  remark: string;
  itemNotes: Array<ItemNote>;
}
