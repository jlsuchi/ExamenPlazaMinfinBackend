import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../servicios/usuario';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  usuario: string = '';
  password: string = '';

  mensajeError = signal('');
  cargando = signal(false);

  constructor(
    private usuarioService: UsuarioService,
    private router: Router
  ) {}

  ingresar(): void {

    this.mensajeError.set('');

    if (!this.usuario || !this.password) {
      this.mensajeError.set(
        'Debe ingresar usuario y contraseña.'
      );
      return;
    }

    this.cargando.set(true);

    this.usuarioService.login({
      usuario: this.usuario,
      password: this.password
    }).subscribe({

      next: (respuesta) => {

        this.cargando.set(false);

        localStorage.setItem(
          'usuario',
          JSON.stringify(respuesta.usuario)
        );

        this.router.navigate(['/inicio']);
      },

      error: (error) => {

        console.log('ERROR LOGIN:', error);

        this.cargando.set(false);

        if (error.status === 401) {

          this.mensajeError.set(
            error.error?.mensaje ||
            'Usuario o contraseña incorrectos.'
          );

        } else {

          this.mensajeError.set(
            'No fue posible conectarse con el servidor.'
          );

        }
      }

    });
  }
}