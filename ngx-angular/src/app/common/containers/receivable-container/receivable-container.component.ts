import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ReceivableNavbarComponent} from "../../navbars/receivable-navbar/receivable-navbar.component";

@Component({
  selector: "app-receivable-container",
  standalone: true,
  imports: [
    ReceivableNavbarComponent
  ],
  templateUrl: "./receivable-container.component.html"
})
export class ReceivableContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
