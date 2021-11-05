import {Component, OnInit} from '@angular/core';
import {faBars, faEnvelope, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-cobrar-modal',
  templateUrl: './cobrar-modal.component.html',
  styleUrls: ['./cobrar-modal.component.scss']
})
export class CobrarModalComponent implements OnInit {
  faBars = faBars;
  faCheckSquare = faCheckSquare;
  faEnvelope = faEnvelope;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  cuotaModal: any;

  constructor() {
  }

  ngOnInit(): void {
    this.cuotaModal = new bootstrap.Modal(document.querySelector('#cuota-modal'));
  }

  // agregar cuota.
  public addCuotaModal(e:any): void {
    e.preventDefault();
    this.cuotaModal.show();
  }

}
