import {ListaBandejaFacturador} from './lista-bandeja-facturador';
import {FacturadorBase} from './facturador-base';

export class FacturadorData extends FacturadorBase {
  constructor(
    public mensaje: string = '',
    public listaBandejaFacturador: Array<ListaBandejaFacturador> = new Array<ListaBandejaFacturador>()) {
    super();
  }
}
