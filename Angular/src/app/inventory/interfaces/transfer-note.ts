import {TransferNoteDetail} from './transfer-note-detail';

export interface TransferNote {
  id: number | null;
  origin: string;
  target: string;
  motivo: string;
  startDate: any;
  remark: string;
  status: string;
  transferNoteDetails: Array<TransferNoteDetail>;
}
