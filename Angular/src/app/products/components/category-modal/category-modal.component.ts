import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
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
  @Output()
  responseData = new EventEmitter<ResponseData<Category>>();
  categoryForm: FormGroup = this.fb.group({
    name: ['']
  });

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#category-modal');
    myModal.addEventListener('hidden.bs.modal', () => {
      this.categoryForm.reset();
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    this.categoryService.store(this.categoryForm.value)
      .subscribe(result => this.responseData.emit(result));
  }

}
