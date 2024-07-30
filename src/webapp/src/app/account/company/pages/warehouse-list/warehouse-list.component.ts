import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute} from "@angular/router";
import {deleteConfirm, toastSuccess} from "app/common/interfaces";
import {Warehouse, WarehouseDataModal} from "../../interfaces";
import {WarehouseService} from "../../services";
import _ from "lodash";
import {
  CompanyDetailContainerComponent
} from "app/common/containers/company-detail-container/company-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {WarehouseModalComponent} from "../../components/warehouse-modal/warehouse-modal.component";
import {NgForOf} from "@angular/common";

declare const bootstrap: any;

@Component({
  selector: "app-warehouse-list",
  standalone: true,
  imports: [
    CompanyDetailContainerComponent,
    FaIconComponent,
    ReactiveFormsModule,
    WarehouseModalComponent,
    NgForOf
  ],
  templateUrl: "./warehouse-list.component.html"
})
export class WarehouseListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  companyId: string = "";
  query: FormControl = this.fb.control("");
  warehouseDataModal = new WarehouseDataModal();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  warehouseModal: any;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      // cargar lista de almacenes.
      this.getWarehouses();
    });
    // formulario modal almacén.
    this.warehouseModal = new bootstrap.Modal(document.querySelector("#warehouse-modal"));
  }

  // obtener lista de almacenes.
  private getWarehouses(): void {
    this.warehouseService.index(this.query.value)
      .subscribe(result => this.warehouses = result);
  }

  // buscar almacén.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.getWarehouses();
  }

  // abrir modal almacén.
  public showWarehouseModal(): void {
    this.warehouseDataModal.type = "ADD";
    this.warehouseDataModal.title = "Agregar Almacén";
    this.warehouseDataModal.warehouse = new Warehouse();
    this.warehouseModal.show();
  }

  // abrir modal almacén modo edición.
  public editWarehouseModal(warehouse: Warehouse): void {
    this.warehouseDataModal.type = "EDIT";
    this.warehouseDataModal.title = "Editar Almacén";
    this.warehouseDataModal.warehouse = warehouse;
    this.warehouseModal.show();
  }

  // ocultar modal almacén.
  public saveChangesDetail(response: WarehouseDataModal): void {
    const {warehouse} = response;
    if (response.type === "ADD") {
      this.warehouseService.create(warehouse)
        .subscribe(result => {
          this.warehouses = _.concat(result, this.warehouses);
          this.warehouseModal.hide();
          toastSuccess("El almacén ha sido registrado!");
        });
    }
    if (response.type === "EDIT") {
      this.warehouseService.update(warehouse.id, warehouse)
        .subscribe(result => {
          this.warehouses = _.map(this.warehouses, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          this.warehouseModal.hide();
          toastSuccess("El almacén ha sido actualizado!");
        });
    }
  }

  // borrar almacén.
  public deleteWarehouse(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.warehouseService.delete(id).subscribe(result => {
          this.warehouses = _.filter(this.warehouses, item => item.id !== result.id);
        });
      }
    });
  }

}
