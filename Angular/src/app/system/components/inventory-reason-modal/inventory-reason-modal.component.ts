import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {InventoryReason} from '../../interfaces';
import {ResponseData} from '../../../global/interfaces';

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
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#inventory-reason-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.inventoryReason !== null) {
        this.inventoryReasonForm.reset(this.inventoryReason);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.inventoryReasonForm.reset();
    });
  }

}
