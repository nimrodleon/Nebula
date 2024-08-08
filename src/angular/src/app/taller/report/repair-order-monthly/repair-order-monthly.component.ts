import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import {
  faEdit,
  faFilter,
  faPlus,
  faSearch,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import { ActivatedRoute, RouterLink } from "@angular/router";
import moment from "moment";
import { RepairOrderService } from "../../services";
import { RepairOrder } from "../../interfaces";
import { UserDataService } from "app/common/user-data.service";
import { TallerContainerComponent } from "app/common/containers/taller-container/taller-container.component";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { DatePipe, NgClass, NgForOf } from "@angular/common";
import { PaginationResult } from "app/common/interfaces";

@Component({
  selector: "app-repair-order-monthly",
  standalone: true,
  imports: [
    TallerContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    DatePipe,
    NgClass,
    NgForOf,
    RouterLink
  ],
  templateUrl: "./repair-order-monthly.component.html"
})
export class RepairOrderMonthlyComponent implements OnInit {
  faPlus = faPlus;
  faFilter = faFilter;
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  companyId: string = "";
  repairOrders: PaginationResult<RepairOrder> = new PaginationResult<RepairOrder>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY"), [Validators.required]],
    month: [moment().format("MM"), [Validators.required]],
    query: ["", [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private repairOrderService: RepairOrderService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.obtenerReporteMensual(page);
    });
  }

  public get companyName(): string {
    return this.userDataService.companyName;
  }

  public obtenerReporteMensual(page: number = 1): void {
    const { year, month, query } = this.queryForm.value;
    this.repairOrderService.getMonthlyReport(year, month, query, page)
      .subscribe(result => this.repairOrders = result);
  }

}
