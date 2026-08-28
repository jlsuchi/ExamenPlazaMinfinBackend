import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cuenta } from '../Modelos/Cuenta';

@Injectable({
  providedIn: 'root'
})
export class CuentaService {
  private apiUrl = 'https://localhost:7187/api/Cuenta';

  constructor(private http: HttpClient) { }

  obtenerCuentas(): Observable<Cuenta[]> {
    return this.http.get<Cuenta[]>(this.apiUrl);
  }
}