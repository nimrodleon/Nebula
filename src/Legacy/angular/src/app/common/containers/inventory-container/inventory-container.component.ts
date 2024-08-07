import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {InventoryNavbarComponent} from "../../navbars/inventory-navbar/inventory-navbar.component";

@Component({
  selector: "app-inventory-container",
  standalone: true,
  imports: [
    InventoryNavbarComponent
  ],
  templateUrl: "./inventory-container.component.html"
})
export class InventoryContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
