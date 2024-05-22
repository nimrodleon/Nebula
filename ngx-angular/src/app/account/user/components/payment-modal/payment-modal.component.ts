import { NgClass, NgFor } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { Company, PagoSuscripcionDataModal } from 'app/account/interfaces';
import { FormType, toastError } from 'app/common/interfaces';
import { PagoSuscripcionService } from 'app/account/services';
import moment from 'moment';
import _ from 'lodash';

@Component({
  selector: 'app-payment-modal',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
    NgFor,
  ],
  templateUrl: './payment-modal.component.html'
})
export class PaymentModalComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private pagoSuscripcionService: PagoSuscripcionService = inject(PagoSuscripcionService);
  @Input()
  userId: string = "";
  @Input()
  empresas: Array<Company> = new Array<Company>();
  @Input()
  data: PagoSuscripcionDataModal = new PagoSuscripcionDataModal();
  @Output()
  responseData = new EventEmitter<PagoSuscripcionDataModal>();

  paymentForm: FormGroup = this.fb.group({
    companyId: ["", [Validators.required]],
    fechaDesde: [moment().format('YYYY-MM-DD'), [Validators.required]],
    fechaHasta: [moment().add(30, 'days').format('YYYY-MM-DD'), [Validators.required]],
    monto: [0, [Validators.required]],
    remark: [null]
  });

  ngOnInit(): void {
    const myModal: any = document.querySelector("#paymentModal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.data.type === FormType.EDIT) {
        this.paymentForm.reset({
          companyId: this.data.pagoSuscripcion.companyId,
          fechaDesde: this.data.pagoSuscripcion.fechaDesde,
          fechaHasta: this.data.pagoSuscripcion.fechaHasta,
          monto: this.data.pagoSuscripcion.monto,
          remark: this.data.pagoSuscripcion.remark
        });
      }
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.paymentForm.reset({
        companyId: "",
        fechaDesde: moment().format('YYYY-MM-DD'),
        fechaHasta: moment().add(30, 'days').format('YYYY-MM-DD'),
        monto: 0,
        remark: null
      });
    });
  }

  public inputIsInvalid(field: string) {
    return this.paymentForm.controls[field].errors
      && this.paymentForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const {
      companyId,
      fechaDesde,
      fechaHasta,
      monto,
      remark,
    } = this.paymentForm.value;
    const empresa = _.find(this.empresas, item => item.id === companyId);
    if (this.data.type === FormType.ADD) {
      this.pagoSuscripcionService.create({
        ...this.data.pagoSuscripcion,
        userId: this.userId,
        companyId: companyId || "",
        companyName: empresa?.rznSocial || "",
        fechaDesde: fechaDesde || "",
        fechaHasta: fechaHasta || "",
        monto: monto || 0,
        remark: remark || "",
        createdAt: moment().format("YYYY-MM-DD")
      }).subscribe(result => {
        this.data.pagoSuscripcion = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const { id } = this.data.pagoSuscripcion;
      this.pagoSuscripcionService.update(id, {
        ...this.data.pagoSuscripcion,
        companyId: companyId || "",
        companyName: empresa?.rznSocial || "",
        fechaDesde: fechaDesde || "",
        fechaHasta: fechaHasta || "",
        monto: monto || 0,
        remark: remark || "",
        id
      }).subscribe(result => {
        this.data.pagoSuscripcion = result;
        this.responseData.emit(this.data);
      });
    }
  }

}
