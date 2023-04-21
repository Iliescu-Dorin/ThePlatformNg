import { Component, HostListener, Input, OnInit } from '@angular/core';
import { OnlineStatusService, OnlineStatusType } from 'ngx-online-status';
import { languages, notifications, userItems } from './header-dummy-data';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  status: OnlineStatusType = OnlineStatusType.ONLINE; //Enum provided by ngx-online-status
  onlineStatusCheck = OnlineStatusType;

  canShowSearchAsOverlay = false;
  selectedLanguage : any;

  languages = languages;
  notifications = notifications;
  userItems= userItems;

  @Input() collapsed = false;
  @Input() screenWidth = 0;


  constructor(private onlineStatusService: OnlineStatusService) {
    this.onlineStatusService.status.subscribe((status: OnlineStatusType) => {
      // Retrieve Online status Type
      this.status = status;
    });
  }

  ngOnInit(): void {
    this.checkCanShowSearchAsOverlay(window.innerWidth);
    this.selectedLanguage = this.languages[0];
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.checkCanShowSearchAsOverlay(window.innerWidth);
  }

  changeLanguage(lang : any) {
    this.selectedLanguage = lang;
  }

  getHeaderClass() : string {
    let styleClass = '';
    if(this.collapsed && this.screenWidth > 768) {
      styleClass = 'head-trimmed';
    }
    else {
      styleClass = 'head-md-screen';
    }
    return styleClass;
  }

  checkCanShowSearchAsOverlay(innWidth: number) : void {
    this.canShowSearchAsOverlay = innWidth < 845;
  }

}
