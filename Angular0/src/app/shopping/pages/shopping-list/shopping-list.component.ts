import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.scss']
})
export class ShoppingListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;

  constructor() {
  }

  ngOnInit(): void {
  }

}
