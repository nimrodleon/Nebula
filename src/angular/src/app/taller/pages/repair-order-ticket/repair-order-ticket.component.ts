import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {CurrencyPipe, DatePipe, NgForOf} from "@angular/common";
import {TallerContainerComponent} from "app/common/containers/taller-container/taller-container.component";
import {RepairOrderService} from "../../services";
import {TallerRepairOrderTicket} from "../../interfaces";
import _ from "lodash";

@Component({
  selector: "app-repair-order-ticket",
  standalone: true,
  imports: [
    DatePipe,
    CurrencyPipe,
    TallerContainerComponent,
    NgForOf
  ],
  templateUrl: "./repair-order-ticket.component.html"
})
export class RepairOrderTicketComponent implements OnInit {
  ticket: TallerRepairOrderTicket = new TallerRepairOrderTicket();
  montoTotalMateriales: number = 0;

  constructor(
    private activatedRoute: ActivatedRoute,
    private repairOrderService: RepairOrderService) {
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.params["id"];
    this.repairOrderService.getTicket(id)
      .subscribe(result => {
        this.ticket = result;
        this.calcularMontoTotalMateriales();
      });
  }

  private calcularMontoTotalMateriales(): void {
    this.montoTotalMateriales = _.sumBy(this.ticket.itemsRepairOrder, item => item.monto);
  }

  public print(): void {
    window.print();
  }

}
