import {Component, OnInit} from '@angular/core';
import {faCog} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-system-list',
  templateUrl: './system-list.component.html',
  styleUrls: ['./system-list.component.scss']
})
export class SystemListComponent implements OnInit {
  faCog = faCog;

  constructor() {
  }

  ngOnInit(): void {
  }

}
