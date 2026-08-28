import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Login } from './login/login';
import {Inicio } from './inicio/inicio';

const routes: Routes = [
  { path: '', component: Login },
  { path: 'login', component: Login },
  { path: 'inicio', component: Inicio },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }