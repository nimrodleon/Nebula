import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;

  constructor() {
  }

  ngOnInit(): void {
  }

}
