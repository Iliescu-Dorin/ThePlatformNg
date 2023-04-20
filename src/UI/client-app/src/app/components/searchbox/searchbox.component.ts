import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';

@Component({
  selector: 'app-searchbox',
  templateUrl: './searchbox.component.html',
  styleUrls: ['./searchbox.component.scss'],
})
export class SearchboxComponent {
  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler(event: KeyboardEvent) {
    this.btnClass = '';
    this.iptClass = '';
  }
  @ViewChild('search', {static: false}) searchElement: ElementRef | undefined;
  index = 0;
  btnClass: any;
  iptClass: any;

  tabChange(data: number) {
    this.index = data;
  }
  btnClickHandler() {
    if (this.btnClass) {
      this.btnClass = '';
      this.iptClass = '';
    } else {
      this.btnClass = 'close';
      this.iptClass = 'square';
      setTimeout(() => this.searchElement?.nativeElement.focus(),700);
    }
  }
}
