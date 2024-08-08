import { Component, OnInit } from '@angular/core';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { UserDetailContainerComponent } from '../../components/user-detail-container/user-detail-container.component';
import { faFilter, faPlus, faSearch } from '@fortawesome/free-solid-svg-icons';
import { PaymentModalComponent } from '../../components/payment-modal/payment-modal.component';

declare const bootstrap: any;

@Component({
  selector: 'app-user-suscripciones',
  standalone: true,
  imports: [
    FaIconComponent,
    UserDetailContainerComponent,
    PaymentModalComponent
  ],
  templateUrl: './user-suscripciones.component.html'
})
export class UserSuscripcionesComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  paymentModal: any;

  ngOnInit(): void {
    this.paymentModal = new bootstrap.Modal("#paymentModal");
  }

  public agregarPagoModal() : void {
    this.paymentModal.show();
  }

}
