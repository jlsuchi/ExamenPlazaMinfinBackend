import { TestBed } from '@angular/core/testing';

import { SolicitudPago } from './solicitud-pago';

describe('SolicitudPago', () => {
  let service: SolicitudPago;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SolicitudPago);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
