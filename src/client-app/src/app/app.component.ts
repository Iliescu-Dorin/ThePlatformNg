import { Component, OnInit } from '@angular/core';
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
  title = 'client-app';

  isSideNavCollapsed = false;
  screenWidth=0;

  onToggleSideNav(data: SideNavToggle): void {
    this.screenWidth = data.screenWidth;
    this.isSideNavCollapsed = data.collapsed;
  }
}
