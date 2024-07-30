import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {accessDenied, deleteConfirm, toastSuccess} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import {Category, CategoryDataModal} from "../../interfaces";
import {CategoryService} from "../../services";
import _ from "lodash";
import {ProductContainerComponent} from "app/common/containers/product-container/product-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CategoryModalComponent} from "../../components/category-modal/category-modal.component";
import {NgForOf} from "@angular/common";

declare const bootstrap: any;

@Component({
  selector: "app-category-list",
  standalone: true,
  imports: [
    ProductContainerComponent,
    FaIconComponent,
    ReactiveFormsModule,
    CategoryModalComponent,
    NgForOf
  ],
  templateUrl: "./category-list.component.html"
})
export class CategoryListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  categoryDataModal = new CategoryDataModal();
  categories: Array<Category> = new Array<Category>();
  query: FormControl = this.fb.control("");
  categoryModal: any;

  constructor(
    private fb: FormBuilder,
    private userDataService: UserDataService,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    // modal categoría.
    this.categoryModal = new bootstrap.Modal("#category-modal");
    // cargar lista de categorías.
    this.getCategories();
  }

  public get companyName(): string {
    return this.userDataService.localName;
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
    this.categoryDataModal.type = "ADD";
    this.categoryDataModal.title = "Agregar Categoría";
    this.categoryDataModal.category = new Category();
    this.categoryModal.show();
  }

  // abrir modal categoría modo edición.
  public editCategoryModal(category: Category): void {
    this.categoryDataModal.type = "EDIT";
    this.categoryDataModal.title = "Editar Categoría";
    this.categoryDataModal.category = category;
    this.categoryModal.show();
  }

  // ocultar modal categoría.
  public saveChangesDetail(response: CategoryDataModal): void {
    const {category} = response;
    if (response.type === "ADD") {
      category.id = undefined;
      this.categoryService.create(category)
        .subscribe(result => {
          this.categories = _.concat(result, this.categories);
          this.categoryModal.hide();
          toastSuccess("La categoría ha sido registrado!");
        });
    }
    if (response.type === "EDIT") {
      this.categoryService.update(category?.id, category)
        .subscribe(result => {
          this.categories = _.map(this.categories, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          this.categoryModal.hide();
          toastSuccess("La categoría ha sido actualizado!");
        });
    }
  }

  // borrar categoría.
  public deleteCategory(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.categoryService.delete(id)
            .subscribe(result => {
              this.categories = _.filter(this.categories, item => item.id !== result.id);
            });
        }
      });
    }
  }

}
