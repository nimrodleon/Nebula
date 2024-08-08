import {Component, Input} from "@angular/core";
import {InventoryService} from "../../services";
import moment from "moment/moment";
import {
  faBox,
  faLocationCrosshairs,
  faNoteSticky,
  faRetweet,
  faRightFromBracket
} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, RouterLink, RouterLinkActive} from "@angular/router";
import {
  InventoryContainerComponent
} from "app/common/containers/inventory-container/inventory-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-inventory-detail-container",
  standalone: true,
  imports: [
    RouterLink,
    InventoryContainerComponent,
    FaIconComponent,
    RouterLinkActive
  ],
  templateUrl: "./inventory-detail-container.component.html"
})
export class InventoryDetailContainerComponent {
  faNoteSticky = faNoteSticky;
  faRetweet = faRetweet;
  faBox = faBox;
  faLocationCrosshairs = faLocationCrosshairs;
  faRightFromBracket = faRightFromBracket;
  companyId: string = "";

  @Input()
  type: string = "notas";

  constructor(
    private route: ActivatedRoute,
    private inventoryService: InventoryService,) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    const year = moment().format("YYYY");
    const month = moment().format("MM");
    if (this.type === "notas") {
      this.inventoryService.getInventoryNotas(year, month);
    }
    if (this.type === "transferencia") {
      this.inventoryService.getTransferencias(year, month);
    }
    if (this.type === "ajustes") {
      this.inventoryService.getAjusteInventarios(year, month);
    }
    if (this.type === "materiales") {
      this.inventoryService.getMaterials(year, month);
    }
  }

}
