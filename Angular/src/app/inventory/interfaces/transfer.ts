import {ItemNote} from './item-note';

export interface Transfer {
  origin: string;
  target: string;
  motivo: string;
  startDate: any;
  remark: string;
  itemNotes: Array<ItemNote>;
}
