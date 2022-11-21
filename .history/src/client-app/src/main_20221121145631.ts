import "@angular/compiler";
import { enableProdMode } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import "zone.js";
import { AppComponent } from './app.component';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent)
    .catch(err => console.error(err));
