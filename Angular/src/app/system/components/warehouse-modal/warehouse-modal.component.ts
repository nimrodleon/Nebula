import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
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
  warehouse: Warehouse = new Warehouse();
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
    // cargar valores por defecto.
    const myModal: any = document.querySelector('#warehouse-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.warehouse !== null) {
        this.warehouseForm.reset(this.warehouse);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.warehouseForm.addControl('id', new FormControl(null));
      this.warehouseForm.reset();
    });
  }

  // guardar los cambios establecidos.
  public saveChanges(): void {
    if (this.warehouseForm.get('id')?.value === null) {
      this.warehouseForm.removeControl('id');
      this.warehouseService.store(this.warehouseForm.value)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    } else {
      const id: string = this.warehouseForm.get('id')?.value;
      this.warehouseService.update(id, this.warehouseForm.value)
        .subscribe(result => {
          this.responseData.emit(result);
        });
    }
  }

}
