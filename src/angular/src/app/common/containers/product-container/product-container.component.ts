import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ProductNavbarComponent} from "../../navbars/product-navbar/product-navbar.component";

@Component({
  selector: "app-product-container",
  standalone: true,
  imports: [
    ProductNavbarComponent
  ],
  templateUrl: "./product-container.component.html"
})
export class ProductContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
