import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {InventoryReason} from '../../interfaces';
import {ResponseData} from '../../../global/interfaces';
import {InventoryReasonService} from '../../services';

@Component({
  selector: 'app-inventory-reason-modal',
  templateUrl: './inventory-reason-modal.component.html',
  styleUrls: ['./inventory-reason-modal.component.scss']
})
export class InventoryReasonModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';
  @Input()
  inventoryReason: InventoryReason | any;
  @Output()
  responseData = new EventEmitter<ResponseData<InventoryReason>>();
  inventoryReasonForm: FormGroup = this.fb.group({
    id: [null],
    description: [''],
    type: ['']
  });

  constructor(
    private fb: FormBuilder,
    private inventoryReasonService: InventoryReasonService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#inventory-reason-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.inventoryReason !== null) {
        this.inventoryReasonForm.reset(this.inventoryReason);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.inventoryReasonForm.addControl('id', new FormControl(null));
      this.inventoryReasonForm.reset();
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.inventoryReasonForm.get('id')?.value === null) {
      this.inventoryReasonForm.removeControl('id');
      this.inventoryReasonService.create(this.inventoryReasonForm.value)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.inventoryReasonForm.get('id')?.value;
      this.inventoryReasonService.update(id, this.inventoryReasonForm.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
