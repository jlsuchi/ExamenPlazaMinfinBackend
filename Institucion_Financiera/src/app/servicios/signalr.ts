import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection!: signalR.HubConnection;
  estadoSolicitud = new Subject<any>();

  iniciarConexion(): void {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7187/solicitudPagoHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR conectado'))
      .catch(error => console.error('Error SignalR:', error));

    this.hubConnection.on('SolicitudEstado', (data) => {
      console.log('SignalR recibió:', data);
      this.estadoSolicitud.next(data);
    });
  }
}