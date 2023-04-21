import { TestBed } from '@angular/core/testing';
import { DownloadPwaButtonComponent } from './download-pwa-button.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        DownloadPwaButtonComponent
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(DownloadPwaButtonComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'angular-custom-install-pwa'`, () => {
    const fixture = TestBed.createComponent(DownloadPwaButtonComponent);
    const app:DownloadPwaButtonComponent = fixture.componentInstance;
    expect(app.title).toEqual('angular-custom-install-pwa');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(DownloadPwaButtonComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.content span').textContent).toContain('angular-custom-install-pwa app is running!');
  });
});
