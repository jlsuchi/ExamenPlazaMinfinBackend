import { Component, OnInit, Inject, PLATFORM_ID, ChangeDetectorRef } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../servicios/usuario';
import { CuentaService } from '../servicios/cuenta';
import { SolicitudPagoService } from '../servicios/solicitud-pago';
import { Cuenta } from '../Modelos/Cuenta';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inicio.html',
  styleUrl: './inicio.css'
})
export class Inicio implements OnInit {
  nombre: string = '';
  usuario: string = '';
  idUsuario: number = 0;
  entidades: any[] = [];
  entidadSeleccionada: any = null;
  cuentas: Cuenta[] = [];
  cuentaSeleccionada: Cuenta | null = null;
  solicitudes: any[] = [];
  mostrarFormulario: boolean = false;
  editando: boolean = false;
  historial: any[] = [];
mostrarHistorial: boolean = false;

  solicitud: any = {
    idSolicitudPago: 0,
    entidad: 0,
    unidadEjecutora: 0,
    unidadDesconcentrada: 0,
    idUsuario: 0,
    idCuenta: 0,
    descripcion: '',
    monto: 0
  };

  constructor(
    private router: Router,
    private usuarioService: UsuarioService,
    private cuentaService: CuentaService,
    private solicitudPagoService: SolicitudPagoService,
    private cd: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const usuarioGuardado = localStorage.getItem('usuario');

      if (usuarioGuardado) {
        const datosUsuario = JSON.parse(usuarioGuardado);
        this.nombre = datosUsuario.nombre;
        this.usuario = datosUsuario.usuario;
        this.idUsuario = datosUsuario.idUsuario;
       this.cargarEntidades();
       this.cargarSolicitudes();
             } else {
      this.router.navigate(['/login']);
       }
    }
  }

  cargarEntidades(): void {
    this.usuarioService.obtenerEntidades(this.usuario).subscribe({
      next: (respuesta) => {
        this.entidades = respuesta;
        if (this.entidades.length > 0) this.entidadSeleccionada = this.entidades[0];
        this.cd.detectChanges();
      },
      error: (error) => console.error('Error al consultar las entidades:', error)
    });
  }

  cargarCuentas(): void {
    this.cuentaService.obtenerCuentas().subscribe({
      next: (respuesta) => {
        this.cuentas = respuesta;
        this.cd.detectChanges();
      },
      error: (error) => console.error('Error al consultar las cuentas:', error)
    });
  }

  cargarSolicitudes(): void {
    this.solicitudPagoService.obtenerTodos().subscribe({
      next: (respuesta) => {
        this.solicitudes = respuesta;
        this.cd.detectChanges();
      },
      error: (error) => console.error('Error al consultar solicitudes:', error)
    });
  }

  cambiarEntidad(): void {
    console.log('Entidad seleccionada:', this.entidadSeleccionada);
  }

  cambiarCuenta(): void {
    this.cuentaSeleccionada = this.cuentas.find(x => x.idCuenta === this.solicitud.idCuenta) || null;
  }

  nuevaSolicitud(): void {
    if (!this.entidadSeleccionada) return;

    this.editando = false;
    this.mostrarFormulario = true;
    this.cuentaSeleccionada = null;

    this.solicitud = {
      idSolicitudPago: 0,
      entidad: this.entidadSeleccionada.entidad,
      unidadEjecutora: this.entidadSeleccionada.unidadEjecutora,
      unidadDesconcentrada: this.entidadSeleccionada.unidadDesconcentrada,
      idUsuario: this.idUsuario,
      idCuenta: 0,
      descripcion: '',
      monto: 0
    };

    this.cargarCuentas();
  }

  guardarSolicitud(): void {
    if (this.solicitud.idCuenta === 0) {
      alert('Debe seleccionar una cuenta.');
      return;
    }

    if (this.solicitud.monto <= 0) {
      alert('El monto debe ser mayor a cero.');
      return;
    }

    if (this.editando) {
      this.solicitudPagoService.actualizar(this.solicitud.idSolicitudPago, this.solicitud).subscribe({
        next: () => {
          alert('Solicitud actualizada correctamente.');
          this.mostrarFormulario = false;
          this.editando = false;
          this.cargarSolicitudes();
        },
        error: (error) => console.error('Error al actualizar:', error)
      });
    } else {
      this.solicitudPagoService.insertar(this.solicitud).subscribe({
        next: () => {
          alert('Solicitud registrada correctamente.');
          this.mostrarFormulario = false;
          this.cargarSolicitudes();
        },
        error: (error) => console.error('Error al guardar:', error)
      });
    }
  }

  editarSolicitud(item: any): void {
    this.solicitud = { ...item };
    this.editando = true;
    this.mostrarFormulario = true;

    this.cuentaService.obtenerCuentas().subscribe({
      next: (respuesta) => {
        this.cuentas = respuesta;
        this.cambiarCuenta();
        this.cd.detectChanges();
      },
      error: (error) => console.error('Error al consultar cuentas:', error)
    });
  }

  eliminarSolicitud(id: number): void {
    if (!confirm('¿Desea eliminar esta solicitud?')) return;
    this.solicitudPagoService.eliminar(id).subscribe({
      next: () => {
        alert('Solicitud eliminada correctamente.');
        this.cargarSolicitudes();
          this.cd.detectChanges();
      },
      error: (error) => console.error('Error al eliminar:', error)
    });
  }

  cancelar(): void {
    this.mostrarFormulario = false;
    this.editando = false;
    this.cuentaSeleccionada = null;
  }

  cerrarSesion(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('usuario');
      this.router.navigate(['/login']);
    }
  }

  solicitarPago(idSolicitudPago: number): void {
  if (!confirm('¿Desea solicitar este pago?')) return;

  this.solicitudPagoService.solicitarPago(idSolicitudPago).subscribe({
    next: (respuesta) => {
      alert(respuesta.mensaje || 'Solicitud enviada correctamente.');
      this.cargarSolicitudes();
      this.cd.detectChanges();
    },
    error: (error) => {
      console.error('Error al solicitar pago:', error);
      alert(error.error?.mensaje || 'Error al solicitar el pago.');
    }
  });
}

verHistorial(idSolicitudPago: number): void {
  this.solicitudPagoService.obtenerHistorial(idSolicitudPago).subscribe({
    next: (respuesta) => {
      this.historial = respuesta;
      this.mostrarHistorial = true;
      this.cd.detectChanges();
    },
    error: (error) => {
      console.error('Error al consultar historial:', error);
      alert('Error al consultar el historial.');
    }
  });
}

cerrarHistorial(): void {
  this.mostrarHistorial = false;
  this.historial = [];
}

}