import {
  animate,
  keyframes,
  style,
  transition,
  trigger
} from '@angular/animations';
import { Component } from '@angular/core';
import { interval } from 'rxjs';
import { take } from 'rxjs/operators';
import { PwaService } from './pwa.service';

@Component({
  selector: 'app-download-pwa-button',
  templateUrl: './download-pwa-button.component.html',
  styleUrls: ['./download-pwa-button.component.scss'],
  animations: [
    trigger('fadeSlideIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('500ms', style({ opacity: 1, transform: 'translateY(0px)' })),
      ]),
    ]),
    trigger('press', [
      transition('false => true', [
        animate(
          '750ms cubic-bezier(0.770, 0.000, 0.175, 1.000)',
          keyframes([
            style({ transform: 'scale(1)', offset: 0 }),
            style({ transform: 'scale(0.9)', offset: 0.5 }),
            style({ transform: 'scale(1)', offset: 1 }),
          ])
        ),
      ]),
    ]),
    trigger('SlideInForbouce', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('280ms', style({ opacity: 1, transform: 'translateY(-6px)' })),
      ]),
    ]),
  ],
})

export class DownloadPwaButtonComponent {
  title = 'angular-custom-install-pwa';
  showButton = false;
  disabled = false;
  pressed = false;
  progressState = false;
  doneState = false;
  bounce = false;
  zoom = false;
  initial = true;
  takeFourSeconds = interval(2000).pipe(take(4));


   constructor( public pwa: PwaService ) {
    // let result=null;
    if(pwa.shouldInstall()){
      this.showButton = true;
    }
    // result = pwa.checkIfPwaInstalled();
    // result.then((res) => {
    //   if(res){
    //     this.disabled = true;
    //   }
    // });
  }

  buttonClicked() {
    this.pressed = true;
    this.pwa.installPwa()
    this.takeFourSeconds.subscribe((x) => {
      if (x == 0) {
        this.initial = false;
        this.pressed = false;
        this.progressState = true;
        this.bounce = true;
      }
      if (x == 2) {
        this.doneState = true;
        this.zoom = true;
        this.bounce = false;
      }
      if (x == 3) {
        this.progressState = this.doneState = this.zoom = false;
        this.initial = true;
      }
    });
    this.disabled = true;
  }
}
