import { NgClass } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import moment from 'moment';

@Component({
  selector: 'app-payment-modal',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
  ],
  templateUrl: './payment-modal.component.html'
})
export class PaymentModalComponent {
  private fb: FormBuilder = inject(FormBuilder);

  paymentForm: FormGroup = this.fb.group({
    desde: [moment().format('YYYY-MM-DD'), [Validators.required]],
    hasta: [moment().add(30, 'days').format('YYYY-MM-DD'), [Validators.required]],
    monto: [0, [Validators.required]],
    remark: [null]
  });

  public inputIsInvalid(field: string) {
    return this.paymentForm.controls[field].errors
      && this.paymentForm.controls[field].touched;
  }


}
