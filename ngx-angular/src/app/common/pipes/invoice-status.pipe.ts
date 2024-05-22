import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "invoiceStatus"
})
export class InvoiceStatusPipe implements PipeTransform {

  transform(value: boolean): unknown {
    return value ? "ACEPTADO" : "RECHAZADO";
  }

}
