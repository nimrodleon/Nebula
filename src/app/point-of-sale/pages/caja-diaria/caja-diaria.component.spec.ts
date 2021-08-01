import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CajaDiariaComponent } from './caja-diaria.component';

describe('CajaDiariaComponent', () => {
  let component: CajaDiariaComponent;
  let fixture: ComponentFixture<CajaDiariaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CajaDiariaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CajaDiariaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
