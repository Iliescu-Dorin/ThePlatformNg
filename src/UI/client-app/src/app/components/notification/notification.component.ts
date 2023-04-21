import { Component } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent {
  title = 'increment-notification';

  notify = false;
  count = 4;

  onSendClick(){
    this.count++;
    this.notify = true;

    setTimeout(() => {
      this.notify = false;
    }
    , 300);
  }
}
