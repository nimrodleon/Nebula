import {Component, Input, OnInit} from '@angular/core';
import {faBars, faSearch} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-product-modal',
  templateUrl: './product-modal.component.html',
  styleUrls: ['./product-modal.component.scss']
})
export class ProductModalComponent implements OnInit {
  faBars = faBars;
  faSearch = faSearch;
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
