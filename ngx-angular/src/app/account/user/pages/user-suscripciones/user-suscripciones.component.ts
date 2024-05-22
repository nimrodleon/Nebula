import { Component, OnInit, inject } from '@angular/core';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { UserDetailContainerComponent } from '../../components/user-detail-container/user-detail-container.component';
import { faEdit, faFilter, faPlus, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { PaymentModalComponent } from '../../components/payment-modal/payment-modal.component';
import { Company, PagoSuscripcion, PagoSuscripcionDataModal } from 'app/account/interfaces';
import { UserService } from '../../services';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PagoSuscripcionService } from 'app/account/services';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormType, PaginationResult, accessDenied, deleteConfirm } from 'app/common/interfaces';
import { CurrencyPipe, DatePipe, NgClass, NgFor } from '@angular/common';
import { UserDataService } from 'app/common/user-data.service';
import moment from 'moment';
import _ from 'lodash';

declare const bootstrap: any;

@Component({
  selector: 'app-user-suscripciones',
  standalone: true,
  imports: [
    NgFor,
    NgClass,
    DatePipe,
    CurrencyPipe,
    RouterLink,
    FaIconComponent,
    ReactiveFormsModule,
    UserDetailContainerComponent,
    PaymentModalComponent
  ],
  templateUrl: './user-suscripciones.component.html'
})
export class UserSuscripcionesComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private userService: UserService = inject(UserService);
  private userDataService: UserDataService = inject(UserDataService);
  private pagoSuscripcionService: PagoSuscripcionService = inject(PagoSuscripcionService);
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  paymentModal: any;
  pagoSuscripcionDataModal: PagoSuscripcionDataModal = new PagoSuscripcionDataModal();
  userId: string = "";
  empresas: Array<Company> = new Array<Company>();
  pagos: PaginationResult<PagoSuscripcion> = new PaginationResult<PagoSuscripcion>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY"), [Validators.required]],
    query: [""]
  });

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = params.get("userId") || "";
      this.cargarPagosSuscripcion();
    });
    this.paymentModal = new bootstrap.Modal("#paymentModal");
  }

  public cargarPagosSuscripcion(page: number = 1): void {
    const { year, query } = this.queryForm.value;
    this.pagoSuscripcionService.index(this.userId, year, query, page)
      .subscribe(result => this.pagos = result);
  }

  public agregarPagoModal(): void {
    this.pagoSuscripcionDataModal.type = FormType.ADD;
    this.pagoSuscripcionDataModal.pagoSuscripcion = new PagoSuscripcion();
    this.userService.getCompanies(this.userId)
      .subscribe(result => {
        this.empresas = result;
        this.paymentModal.show();
      });
  }

  public editarPagoSuscripcionModal(item: PagoSuscripcion): void {
    this.pagoSuscripcionDataModal.type = FormType.EDIT;
    this.pagoSuscripcionDataModal.pagoSuscripcion = item;
    this.userService.getCompanies(this.userId)
      .subscribe(result => {
        this.empresas = result;
        this.paymentModal.show();
      });
  }

  public responseModal(data: PagoSuscripcionDataModal): void {
    if (data.type === FormType.ADD) {
      this.pagos.data = _.concat(data.pagoSuscripcion, this.pagos.data);
      this.paymentModal.hide();
    }
    if (data.type === FormType.EDIT) {
      this.pagos.data = _.map(this.pagos.data, item => {
        if (item.id === data.pagoSuscripcion.id) item = data.pagoSuscripcion;
        return item;
      });
      this.paymentModal.hide();
    }
  }

  public borrarPagoSuscripcion(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.pagoSuscripcionService.delete(id)
            .subscribe(result => {
              this.pagos.data = _.filter(this.pagos.data, item => item.id !== result.id);
            });
        }
      });
    }
  }

}
