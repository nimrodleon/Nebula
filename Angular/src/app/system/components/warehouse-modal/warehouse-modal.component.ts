import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {WarehouseService} from '../../services';
import {ResponseData} from '../../../global/interfaces';
import {Warehouse} from '../../interfaces';

@Component({
  selector: 'app-warehouse-modal',
  templateUrl: './warehouse-modal.component.html',
  styleUrls: ['./warehouse-modal.component.scss']
})
export class WarehouseModalComponent implements OnInit {
  @Input()
  title: string = '';
  @Input()
  warehouse: Warehouse | any;
  @Output()
  responseData = new EventEmitter<ResponseData<Warehouse>>();
  faBars = faBars;
  warehouseForm: FormGroup = this.fb.group({
    id: [null],
    name: [''],
    remark: ['']
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
  }

  public saveChanges(): void {
    if (this.warehouseForm.get('id')?.value === null) {
      this.warehouseForm.removeControl('id');
      this.warehouseService.store(this.warehouseForm.value)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    } else {

    }
  }

}
