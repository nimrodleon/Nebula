import {Component, Input, OnInit} from '@angular/core';
import {faBars} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-contact-modal',
  templateUrl: './contact-modal.component.html',
  styleUrls: ['./contact-modal.component.scss']
})
export class ContactModalComponent implements OnInit {
  faBars = faBars;
  // ====================================================================================================
  @Input()
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
