import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "catalogo6"
})
export class Catalogo6Pipe implements PipeTransform {

  transform(value: string): string {
    let result: string = "";
    switch (value) {
      case "0":
        result = "SIN.RUC";
        break;
      case "1":
        result = "D.N.I";
        break;
      case "4":
        result = "CARNET EXT";
        break;
      case "6":
        result = "R.U.C";
        break;
      case "7":
        result = "PASAPORTE";
        break;
    }
    return result;
  }

}
