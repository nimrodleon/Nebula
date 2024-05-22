import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "tipoMoneda"
})
export class TipoMonedaPipe implements PipeTransform {

  transform(value: string): string {
    let result: string = "";
    switch (value) {
      case "PEN":
        result = "SOLES";
        break;
    }
    return result;
  }

}
