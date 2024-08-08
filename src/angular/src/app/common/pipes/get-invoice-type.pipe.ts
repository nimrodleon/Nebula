import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "getInvoiceType"
})
export class GetInvoiceTypePipe implements PipeTransform {

  transform(value: string): string {
    let result: string = "";
    switch (value) {
      case "07":
        result = "NOTA DE CRÉDITO";
        break;
      case "03":
        result = "BOLETA";
        break;
      case "01":
        result = "FACTURA";
        break;
    }
    return result;
  }

}
