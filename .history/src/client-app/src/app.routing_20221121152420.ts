
import "@angular/compiler";
import { NgZone } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import "zone.js";
import { AppComponent } from './app.component';

import { appRouting } from "./app.routing";
import './index.css';

bootstrapApplication(AppComponent, {
    providers: [
        {
            provide: NgZone,
            useValue: new NgZone({ shouldCoalesceEventChangeDetection: false })
        },
        ...appRouting
    ]
});

// import { otherAction } from './other';

async function lazyOther () {
    const { otherAction } = await import('./other');
    document.querySelector<HTMLDivElement>('#app')!.innerHTML = 'Hello world';
    otherAction();
}

