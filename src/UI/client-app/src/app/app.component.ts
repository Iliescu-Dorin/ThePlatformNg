import { Component, OnInit } from '@angular/core';
// import { SwUpdate } from '@angular/service-worker';
import { Observable } from 'rxjs';
import { ThemeService } from './services/theme-service';

interface SideNavToggle{
  screenWidth: number;
  collapsed: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent  {
  // isDarkTheme: Observable<boolean> | undefined;

  // constructor(
  //   private themeService: ThemeService,
  //   ) {}

  // ngOnInit() {
  //   this.isDarkTheme = this.themeService.isDarkTheme;
  // }
  /**
   *
   */
  title = "DreamKatcher";
  // update = false;
  // constructor(updates: SwUpdate) {
  //   updates.versionUpdates.subscribe(event => {
  //     this.update = true;
  //     updates.activateUpdate().then(() => document.location.reload());
  //   });
  // }

  isSideNavCollapsed = false;
  screenWidth=0;
  show = 0;

  onToggleSideNav(data: SideNavToggle): void {
    this.screenWidth = data.screenWidth;
    this.isSideNavCollapsed = data.collapsed;
  }
}
