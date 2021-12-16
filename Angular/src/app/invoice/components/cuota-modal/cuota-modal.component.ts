import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faPlus} from '@fortawesome/free-solid-svg-icons';
import {Cuota} from '../../interfaces';

@Component({
  selector: 'app-cuota-modal',
  templateUrl: './cuota-modal.component.html',
  styleUrls: ['./cuota-modal.component.scss']
})
export class CuotaModalComponent implements OnInit {
  faPlus = faPlus;
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
      this.cuotaForm.reset({endDate: null, amount: 0});
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    this.responseData.emit(this.cuotaForm.value);
  }

}
