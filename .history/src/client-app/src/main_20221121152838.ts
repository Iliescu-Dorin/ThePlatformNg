
import "@angular/compiler";
import { NgZone } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import "zone.js";
import { AppComponent } from './app/app.component';

import { AppRoutingModule } from './app/app-routing.module';
import './index.css';

bootstrapApplication(AppComponent, {
    providers: [
        {
            provide: NgZone,
            useValue: new NgZone({ shouldCoalesceEventChangeDetection: false })
        },
        AppRoutingModule
    ]
});

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
