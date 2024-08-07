import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {SalesNavbarComponent} from "../../navbars/sales-navbar/sales-navbar.component";

@Component({
  selector: "app-sales-container",
  standalone: true,
  imports: [
    SalesNavbarComponent
  ],
  templateUrl: "./sales-container.component.html"
})
export class SalesContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
