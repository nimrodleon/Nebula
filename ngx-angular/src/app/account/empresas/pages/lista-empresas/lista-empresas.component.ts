import { Component } from '@angular/core';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faFilter, faPlus, faSearch } from '@fortawesome/free-solid-svg-icons';
import { AccountContainerComponent } from 'app/common/containers/account-container/account-container.component';

@Component({
  selector: 'app-lista-empresas',
  standalone: true,
  imports: [
    AccountContainerComponent,
    FaIconComponent
  ],
  templateUrl: './lista-empresas.component.html',
})
export class ListaEmpresasComponent {
  faFilter = faFilter;
  faSearch=faSearch;
  faPlus = faPlus;
}
