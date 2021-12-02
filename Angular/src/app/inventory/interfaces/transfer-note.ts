import {TransferNoteDetail} from './transfer-note-detail';
import {Warehouse} from '../../system/interfaces';

export interface TransferNote {
  id: number | null;
  origin: Warehouse | any;
  originId: string;
  target: Warehouse | any;
  targetId: string;
  motivo: string;
  startDate: any;
  remark: string;
  status: string;
  transferNoteDetails: Array<TransferNoteDetail>;
}
