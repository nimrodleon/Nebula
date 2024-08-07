import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "invoiceType"
})
export class InvoiceTypePipe implements PipeTransform {

  transform(value: string): string {
    let result: string = "";
    switch (value) {
      case "03":
        result = "BOLETA DE VENTA ELECTRÓNICA";
        break;
      case "01":
        result = "FACTURA ELECTRÓNICA";
        break;
      case "NOTA":
        result = "NOTA DE VENTA";
        break;
    }
    return result;
  }

}
