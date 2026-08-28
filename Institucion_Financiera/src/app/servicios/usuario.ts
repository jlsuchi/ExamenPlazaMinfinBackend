import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login } from '../Modelos/login';
import { LoginRespuesta } from '../Modelos/login-respuesta';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private apiUrl = 'https://localhost:7187/api/Usuario';

  constructor(private http: HttpClient) { }

  login(login: Login): Observable<LoginRespuesta> {
    return this.http.post<LoginRespuesta>(
      `${this.apiUrl}/login`,
      login
    );
  }

    obtenerEntidades(usuario: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/entidades/${usuario}`);
  }
}