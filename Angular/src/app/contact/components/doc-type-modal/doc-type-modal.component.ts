import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-doc-type-modal',
  templateUrl: './doc-type-modal.component.html',
  styleUrls: ['./doc-type-modal.component.scss']
})
export class DocTypeModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
