import {
  animate,
  keyframes,
  style,
  transition,
  trigger
} from '@angular/animations';
import {
  Component,
  EventEmitter,
  HostListener,
  OnInit,
  Output
} from '@angular/core';
import { navbarData } from './nav-data';

interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('350ms', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        style({ opacity: 1 }),
        animate('350ms', style({ opacity: 0 })),
      ]),
    ]),
    trigger('rotate', [
      transition(':enter', [
        animate('1000ms',
        keyframes([
          style({ transform: 'rotate(0deg)' , offset: '0'}),
          style({ transform: 'rotate(2turn)' , offset: '1'}),
        ])),
      ]),
    ]),
  ],
})
export class SidenavComponent implements OnInit {
  @Output() toggleSideNav: EventEmitter<SideNavToggle> = new EventEmitter();
  collapsed = false; // default value
  screenWidth = 0;
  navData = navbarData;

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.screenWidth = window.innerWidth;
    if (this.screenWidth <= 768) {
      this.collapsed = false;
      this.toggleSideNav.emit({
        screenWidth: this.screenWidth,
        collapsed: this.collapsed,
      });
    }
  }

  ngOnInit(): void {
    this.screenWidth = window.innerWidth;
  }

  toggleCollapse(): void {
    this.collapsed = !this.collapsed;
    this.toggleSideNav.emit({
      screenWidth: this.screenWidth,
      collapsed: this.collapsed,
    });
  }

  closeSideNav() {
    this.collapsed = false;
    this.toggleSideNav.emit({
      screenWidth: this.screenWidth,
      collapsed: this.collapsed,
    });
  }
}
