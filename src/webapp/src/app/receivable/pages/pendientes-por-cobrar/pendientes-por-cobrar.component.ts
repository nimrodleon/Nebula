import {CurrencyPipe, DatePipe} from "@angular/common";
import {Component, OnInit} from "@angular/core";
import {
  ReceivableContainerComponent
} from "app/common/containers/receivable-container/receivable-container.component";
import {ResumenDeuda} from "app/receivable/interfaces";
import {ReceivableService} from "app/receivable/services";

@Component({
  selector: "app-pendientes-por-cobrar",
  standalone: true,
  imports: [
    ReceivableContainerComponent,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: "./pendientes-por-cobrar.component.html",
})
export class PendientesPorCobrarComponent implements OnInit {
  resumenDeuda = new ResumenDeuda();

  constructor(private receivableService: ReceivableService) {
  }

  ngOnInit(): void {
    this.receivableService.getPendientesPorCobrar()
      .subscribe(result => {
        this.resumenDeuda = result;
      });
  }

}
