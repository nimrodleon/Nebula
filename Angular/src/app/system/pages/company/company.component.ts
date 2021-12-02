import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faClipboardList, faSave, faThumbtack} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit {
  faClipboardList = faClipboardList;
  faThumbtack = faThumbtack;
  faArrowLeft = faArrowLeft;
  faSave = faSave;

  constructor() {
  }

  ngOnInit(): void {
  }

}
