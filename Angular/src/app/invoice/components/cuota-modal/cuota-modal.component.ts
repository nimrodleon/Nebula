import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars, faPlus} from '@fortawesome/free-solid-svg-icons';
import {Cuota} from '../../interfaces';

@Component({
  selector: 'app-cuota-modal',
  templateUrl: './cuota-modal.component.html',
  styleUrls: ['./cuota-modal.component.scss']
})
export class CuotaModalComponent implements OnInit {
  faPlus = faPlus;
  faBars = faBars;
  @Input()
  cuota: Cuota = new Cuota();
  cuotaForm: FormGroup = this.fb.group({
    endDate: [null],
    amount: [0],
  });
  @Output()
  responseData = new EventEmitter<Cuota>();

  constructor(
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#cuota-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      this.cuotaForm.reset({...this.cuota});
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    this.cuota = {...this.cuota, ...this.cuotaForm.value};
    this.responseData.emit(this.cuota);
  }

}
