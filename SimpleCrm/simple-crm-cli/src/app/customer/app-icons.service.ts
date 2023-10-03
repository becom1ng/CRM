import { Injectable } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class AppIconsService {

  constructor(
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer
  ) {
      this.iconRegistry.addSvgIcon('user', this.sanitizer.bypassSecurityTrustResourceUrl('assets/user.svg'));
      this.iconRegistry.addSvgIcon('question', this.sanitizer.bypassSecurityTrustResourceUrl('assets/user-question.svg'));
      this.iconRegistry.addSvgIcon('shield', this.sanitizer.bypassSecurityTrustResourceUrl('assets/shield-dollar.svg'));
    }
}