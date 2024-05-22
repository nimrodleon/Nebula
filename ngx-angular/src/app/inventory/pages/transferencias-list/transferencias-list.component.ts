import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faEdit, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import moment from "moment/moment";
import {InventoryService} from "../../services";
import {
  InventoryDetailContainerComponent
} from "../../components/inventory-detail-container/inventory-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: "app-transferencias-list",
  standalone: true,
  imports: [
    InventoryDetailContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    RouterLink,
    DatePipe,
    NgForOf
  ],
  templateUrl: "./transferencias-list.component.html"
})
export class TransferenciasListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  companyId: string = "";
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private inventoryService: InventoryService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }

  public get transferencias() {
    return this.inventoryService.transferencias;
  }

  public submit(): void {
    const {year, month} = this.queryForm.value;
    this.inventoryService.getTransferencias(year, month);
  }

  public delete(id: string): void {
    this.inventoryService.deleteTransferencia(id);
  }

}
