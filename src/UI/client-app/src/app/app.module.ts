import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, isDevMode, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { CdkMenuModule } from '@angular/cdk/menu';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceWorkerModule } from '@angular/service-worker';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { OnlineStatusModule } from 'ngx-online-status';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BlogComponent } from './blog/blog.component';
import { BodyComponent } from './body/body.component';
import { DownloadPwaButtonComponent } from './components/download-pwa-button/download-pwa-button.component';
import { PwaService } from './components/download-pwa-button/pwa.service';
import { FloatingButtonComponent } from './components/floating-button/floating-button.component';
import { HeaderComponent } from './components/header/header.component';
import { NotificationComponent } from './components/notification/notification.component';
import { SearchboxComponent } from './components/searchbox/searchbox.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { SplashScreenComponent } from './components/splash-screen/splash-screen.component';
import { CulturesComponent } from './cultures/cultures.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyScoreComponent } from './my-score/my-score.component';
import { MyStoriesComponent } from './my-stories/my-stories.component';
import { PagesComponent } from './pages/pages.component';
import { SettingsComponent } from './settings/settings.component';
import { SharedModule } from './shared/shared.module';
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
    MyStoriesComponent,
    SplashScreenComponent,
    SearchboxComponent,
    NotificationComponent,
    FloatingButtonComponent,
  ],
  imports: [
    BrowserModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    OnlineStatusModule,
    SharedModule,
    CdkMenuModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    AppRoutingModule,
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
