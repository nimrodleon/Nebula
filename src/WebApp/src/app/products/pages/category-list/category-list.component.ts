import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {accessDenied, deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {AuthService} from 'src/app/user/services';
import {Category} from '../../interfaces';
import {CategoryService} from '../../services';

declare var bootstrap: any;

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss']
})
export class CategoryListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  currentCategory: Category = new Category();
  categories: Array<Category> = new Array<Category>();
  query: FormControl = this.fb.control('');
  categoryModal: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    // modal categoría.
    this.categoryModal = new bootstrap.Modal(document.querySelector('#category-modal'));
    // cargar lista de categorías.
    this.getCategories();
  }

  // cargar lista de categorías.
  private getCategories(): void {
    this.categoryService.index(this.query.value)
      .subscribe(result => this.categories = result);
  }

  // formulario buscar categorías.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.getCategories();
  }

  // abrir modal categoría.
  public showCategoryModal(): void {
    this.title = 'Agregar Categoría';
    this.currentCategory = new Category();
    this.categoryModal.show();
  }

  // abrir modal categoría modo edición.
  public editCategoryModal(id: any): void {
    this.title = 'Editar Categoría';
    this.categoryService.show(id).subscribe(result => {
      this.currentCategory = result;
      this.categoryModal.show();
    });
  }

  // ocultar modal categoría.
  public hideCategoryModal(response: ResponseData<Category>): void {
    if (response.ok) {
      this.getCategories();
      this.categoryModal.hide();
    }
  }

  // borrar categoría.
  public deleteCategory(id: string): void {
    this.authService.getMe().subscribe(async (result) => {
      if (result.role !== 'Admin') {
        await accessDenied();
      } else {
        deleteConfirm().then(result => {
          if (result.isConfirmed) {
            this.categoryService.delete(id).subscribe(result => {
              if (result.ok) this.getCategories();
            });
          }
        });
      }
    });
  }

}
