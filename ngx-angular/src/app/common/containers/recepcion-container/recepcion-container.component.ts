import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecepcionNavbarComponent } from 'app/common/navbars/recepcion-navbar/recepcion-navbar.component';

@Component({
  selector: 'app-recepcion-container',
  standalone: true,
  imports: [
    RecepcionNavbarComponent
  ],
  templateUrl: './recepcion-container.component.html',
})
export class RecepcionContainerComponent {
  private route: ActivatedRoute = inject(ActivatedRoute);
  companyId: string = "";

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
