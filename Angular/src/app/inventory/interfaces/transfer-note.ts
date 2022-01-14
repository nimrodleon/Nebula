import {TransferNoteDetail} from './transfer-note-detail';
import {Warehouse} from '../../system/interfaces';

export class TransferNote {
  constructor(
    public id: number | any = null, //
    public origin: Warehouse | any = undefined, // Almacén Origen.
    public originId: number = 0, // Id Almacén de Origen.
    public target: Warehouse | any = undefined, // Almacén Destino.
    public targetId: number = 0, // Id Almacén de destino.
    public motivo: string = '', // Motivo de Inventario.
    public startDate: any = null, // Fecha de registro.
    public status: string = '', // Estado de la Transferencia.
    public transferNoteDetails: Array<TransferNoteDetail> = new Array<TransferNoteDetail>()) {
  }
}
