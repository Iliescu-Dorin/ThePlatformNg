import { Component } from '@angular/core';


@Component({
  selector: 'app-floating-button',
  templateUrl: './floating-button.component.html',
  styleUrls: ['./floating-button.component.scss']
})
export class FloatingButtonComponent {
  show = false;
  public onFloatClick () {
    console.log('Floating button clicked');
  }
}
