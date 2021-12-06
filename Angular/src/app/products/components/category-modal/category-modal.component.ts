import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {CategoryService} from '../../services';
import {ResponseData} from '../../../global/interfaces';
import {Category} from '../../interfaces';

@Component({
  selector: 'app-category-modal',
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.scss']
})
export class CategoryModalComponent implements OnInit {
  @Input()
  title: string = '';
  @Input()
  category: Category | any;
  @Output()
  responseData = new EventEmitter<ResponseData<Category>>();
  categoryForm: FormGroup = this.fb.group({
    id: [null],
    name: ['']
  });

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#category-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.category !== null) {
        this.categoryForm.reset(this.category);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.categoryForm.addControl('id', new FormControl(null));
      this.categoryForm.reset();
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.categoryForm.get('id')?.value === null) {
      this.categoryForm.removeControl('id');
      this.categoryService.create(this.categoryForm.value)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.categoryForm.get('id')?.value;
      this.categoryService.update(id, this.categoryForm.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
