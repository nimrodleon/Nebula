import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {
  faEdit,
  faFilter,
  faList,
  faPlus,
  faSearch,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgClass, NgForOf} from "@angular/common";
import {RepairOrderService} from "../../services";
import {RepairOrder} from "../../interfaces";
import {UserDataService} from "app/common/user-data.service";
import {PaginationResult, accessDenied, deleteConfirm} from "app/common/interfaces";
import {TallerContainerComponent} from "app/common/containers/taller-container/taller-container.component";

@Component({
  selector: "app-lista-reparaciones",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    RouterLink,
    NgForOf,
    NgClass,
    DatePipe,
    TallerContainerComponent
  ],
  templateUrl: "./lista-reparaciones.component.html"
})
export class ListaReparacionesComponent implements OnInit {
  faList = faList;
  faSearch = faSearch;
  faPlus = faPlus;
  faFilter = faFilter;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  // ====================================================================================================
  repairOrders: PaginationResult<RepairOrder> = new PaginationResult<RepairOrder>();
  query: FormControl = this.fb.control("");

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private repairOrderService: RepairOrderService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.getRepairOrders(page);
    });
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  public submitForm(e: Event): void {
    e.preventDefault();
    this.getRepairOrders();
  }

  /**
   * Cargar Órdenes de Reparación.
   */
  public getRepairOrders(page: number = 1): void {
    this.repairOrderService.index(this.query.value, page)
      .subscribe(result => this.repairOrders = result);
  }

  /**
   * abir el formulario de edición.
   * @param item Orden de Reparación
   */
  public goToEditForm(item: RepairOrder): void {
    this.router.navigate([
      "/", "taller-reparaciones", "orden-reparacion", "edit", item.id
    ]).then(() => console.log(item.id));
  }

  /**
   * Borrar Orden de Reparación.
   * @param id Identificador de la Orden de Reparación
   */
  public deleteItem(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.repairOrderService.delete(id).subscribe(result => {
            this.repairOrders.data = _.filter(this.repairOrders.data, item => item.id !== result.id);
          });
        }
      });
    }
  }

}
