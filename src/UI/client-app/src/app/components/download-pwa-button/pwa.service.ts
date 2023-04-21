import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PwaService {
  private promptEvent: any;
  status = false;

  public initPwaPrompt() {
    window.addEventListener('beforeinstallprompt', (event: any) => {
      event.preventDefault();
      this.promptEvent = event;
      this.setButton(true);
    });
  }

  public setButton(arr: any) {
    return (this.status = arr);
  }

  public installPwa() {
    this.setButton(false);
    this.promptEvent.prompt();
    this.promptEvent.userChoice.then((choiceResult: any) => {
      if (choiceResult.outcome === 'accepted') {
        console.log('User accepted the A2HS prompt');
      } else {
        console.log('User dismissed the A2HS prompt');
      }
      this.promptEvent = null;
    });
  }

  public async checkIfPwaInstalled(){
    let relatedApps=null;
    //check if browser version supports the api
    if ('getInstalledRelatedApps' in window.navigator) {
      relatedApps =  await (navigator as any).getInstalledRelatedApps();
      if(relatedApps) return true;
    }
    return false;
  }

  public shouldInstall(): boolean {
    switch (this.isRunningStandalone()) {
      case 'browser':
        return true;
      case 'standalone':
        return false;
      case 'twa':
        return false;
      default:
        return false;
    }
  }

  public isRunningStandalone(): string {
    const isStandalone = window.matchMedia(
      '(display-mode: standalone)'
    ).matches;
    if (document.referrer.startsWith('android-app://')) {
      return 'twa';
    } else if ((navigator as any).standalone || isStandalone) {
      return 'standalone';
    }
    return 'browser';
  }
}
