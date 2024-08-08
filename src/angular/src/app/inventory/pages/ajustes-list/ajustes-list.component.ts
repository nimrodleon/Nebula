import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faBox, faEdit, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import moment from "moment/moment";
import {InventoryService} from "../../services";
import {
  InventoryDetailContainerComponent
} from "../../components/inventory-detail-container/inventory-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: "app-ajustes-list",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    InventoryDetailContainerComponent,
    FaIconComponent,
    RouterLink,
    DatePipe,
    NgForOf
  ],
  templateUrl: "./ajustes-list.component.html"
})
export class AjustesListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faBox = faBox;
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

  public get ajusteInventarios() {
    return this.inventoryService.ajusteInventarios;
  }

  public submit(): void {
    const {year, month} = this.queryForm.value;
    this.inventoryService.getAjusteInventarios(year, month);
  }

  public delete(id: string): void {
    this.inventoryService.deleteAjusteInventario(id);
  }

}
