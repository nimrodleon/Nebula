import {Cuota} from './cuota';
import {CpeGeneric} from './cpe-generic';

// modelo comprobantes electrónicos.
export class CpeBase extends CpeGeneric {
  constructor(
    public contactId: number | any = null,
    public docType: string = '', // tipo documento.
    public remark: string | any = undefined, // observación.
    public cuotas: Array<Cuota> = new Array<Cuota>(),
    public invoiceSale: any = undefined) {
    super();
  }

  // Agregar cuota al comprobante.
  public addCuota(data: Cuota): void {
    data.numCuota = this.cuotas.length + 1;
    this.cuotas.push(data);
  }

  // borrar item cuota.
  public deleteCuota(numCuota: number): void {
    let deleted: boolean = false;
    this.cuotas.forEach((value: Cuota, index: number, array: Cuota[]) => {
      if (value.numCuota === numCuota) {
        array.splice(index, 1);
        deleted = true;
      }
    });
    if (deleted) {
      for (let i = 0; i < this.cuotas.length; i++) {
        this.cuotas[i].numCuota = i + 1;
      }
    }
  }
}
