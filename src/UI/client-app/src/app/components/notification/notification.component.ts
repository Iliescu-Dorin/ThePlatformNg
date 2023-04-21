import { Component } from '@angular/core';
import { OnlineStatusService, OnlineStatusType } from 'ngx-online-status';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent {
  title = 'increment-notification';

  notify = false;
  count = 4;

  status: OnlineStatusType = OnlineStatusType.ONLINE; //Enum provided by ngx-online-status
  onlineStatusCheck = OnlineStatusType;

  constructor(private onlineStatusService: OnlineStatusService) {
    this.onlineStatusService.status.subscribe((status: OnlineStatusType) => {
      // Retrieve Online status Type
      this.status = status;
    });
  }

  onSendClick(){
    this.count++;
    this.notify = true;

    setTimeout(() => {
      this.notify = false;
    }
    , 300);
  }
}
