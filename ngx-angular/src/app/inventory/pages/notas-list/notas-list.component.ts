import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faEdit, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import moment from "moment/moment";
import {InventoryService} from "../../services";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {
  InventoryDetailContainerComponent
} from "../../components/inventory-detail-container/inventory-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: "app-notas-list",
  standalone: true,
  imports: [
    InventoryDetailContainerComponent,
    ReactiveFormsModule,
    RouterLink,
    FaIconComponent,
    DatePipe,
    NgForOf,
    NgIf
  ],
  templateUrl: "./notas-list.component.html"
})
export class NotasListComponent implements OnInit {
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

  public get inventoryNotas() {
    return this.inventoryService.inventoryNotas;
  }

  public submit(): void {
    const {year, month} = this.queryForm.value;
    this.inventoryService.getInventoryNotas(year, month);
  }

  public delete(id: string): void {
    this.inventoryService.deleteInventoryNotas(id);
  }

}
