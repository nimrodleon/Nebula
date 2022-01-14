import {ItemNote} from './item-note';

export class Note {
  constructor(
    public contactId: number = 0,
    public warehouseId: number = 0,
    public noteType: string = '',
    public motivo: number | string = 0,
    public startDate: any = null,
    public itemNotes: Array<ItemNote> = new Array<ItemNote>()) {
  }
}
