import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';

import { App } from './app';
import { AppRoutingModule } from './app-routing-module';

@NgModule({
  declarations: [
    App
  ],

  imports: [
    BrowserModule,
    AppRoutingModule
  ],

  providers: [
    provideHttpClient()
  ],

  bootstrap: [
    App
  ]
})
export class AppModule { }