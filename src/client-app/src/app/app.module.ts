import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, isDevMode, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BlogComponent } from './blog/blog.component';
import { BodyComponent } from './body/body.component';
import { DownloadPwaButtonComponent } from './components/download-pwa-button/download-pwa-button.component';
import { PwaService } from './components/download-pwa-button/pwa.service';
import { HeaderComponent } from './components/header/header.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { SplashScreenComponent } from './components/splash-screen/splash-screen.component';
import { CulturesComponent } from './cultures/cultures.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyHistoryComponent } from './my-history/my-history.component';
import { MyScoreComponent } from './my-score/my-score.component';
import { PagesComponent } from './pages/pages.component';
import { SettingsComponent } from './settings/settings.component';
import { StatisticsComponent } from './statistics/statistics.component';

const initializer = (pwaService: PwaService) => () => pwaService.initPwaPrompt();
@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    BodyComponent,
    SettingsComponent,
    PagesComponent,
    StatisticsComponent,
    SidenavComponent,
    HeaderComponent,
    DownloadPwaButtonComponent,
    CulturesComponent,
    MyScoreComponent,
    BlogComponent,
    MyHistoryComponent,
    SplashScreenComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    })
  ],
  providers: [
    {provide: APP_INITIALIZER, useFactory: initializer, deps: [PwaService], multi: true},
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AppModule { }
