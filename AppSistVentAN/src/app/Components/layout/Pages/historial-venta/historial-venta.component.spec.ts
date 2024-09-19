import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorialVentaComponent } from './historial-venta.component';

describe('HistorialVentaComponent', () => {
  let component: HistorialVentaComponent;
  let fixture: ComponentFixture<HistorialVentaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HistorialVentaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistorialVentaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
