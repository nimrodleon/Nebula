import {Pipe, PipeTransform} from "@angular/core";
import moment from "moment";

@Pipe({
  standalone: true,
  name: "calcularDiasVencimiento"
})
export class CalcularDiasVencimientoPipe implements PipeTransform {

  transform(value: string): number | "-" {
    const date = new Date();
    const vence: number = moment([
      date.getFullYear(), date.getMonth(), date.getDate()
    ]).diff(moment(value), "days");
    return vence > 0 ? vence : "-";
  }

}
