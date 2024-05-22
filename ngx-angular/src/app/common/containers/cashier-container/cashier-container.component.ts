import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {CashierNavbarComponent} from "../../navbars/cashier-navbar/cashier-navbar.component";

@Component({
  selector: "app-cashier-container",
  standalone: true,
  imports: [
    CashierNavbarComponent
  ],
  templateUrl: "./cashier-container.component.html"
})
export class CashierContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
