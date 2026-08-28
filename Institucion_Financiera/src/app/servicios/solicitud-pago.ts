import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SolicitudPago } from '../Modelos/SolicitudPago';

@Injectable({
  providedIn: 'root'
})
export class SolicitudPagoService {
  private apiUrl = 'https://localhost:7187/api/SolicitudPago';

  constructor(private http: HttpClient) {}

  obtenerTodos(): Observable<SolicitudPago[]> {
    return this.http.get<SolicitudPago[]>(this.apiUrl);
  }

  insertar(solicitud: SolicitudPago): Observable<any> {
    return this.http.post(this.apiUrl, solicitud);
  }

  actualizar(id: number, solicitud: SolicitudPago): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, solicitud);
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  solicitarPago(idSolicitudPago: number): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/solicitar/${idSolicitudPago}`,
      {}
    );
  }
   obtenerHistorial(idSolicitudPago: number) {
    return this.http.get<any[]>(
      `${this.apiUrl}/${idSolicitudPago}/historial`
    );
  }
  
}