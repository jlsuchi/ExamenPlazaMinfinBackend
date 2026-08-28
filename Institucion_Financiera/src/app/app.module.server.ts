import { NgModule, ApplicationRef } from '@angular/core';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { App } from './app';
import { AppModule } from './app-module';
import { serverRoutes } from './app.routes.server';

@NgModule({
  imports: [
    AppModule
  ],
  providers: [
    provideServerRendering(withRoutes(serverRoutes))
  ]
})
export class AppServerModule {

  ngDoBootstrap(appRef: ApplicationRef) {
    appRef.bootstrap(App);
  }

}