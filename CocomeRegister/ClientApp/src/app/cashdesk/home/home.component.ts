import { Component } from '@angular/core';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class CashDeskHomeComponent {
  expressMode: boolean;

  constructor(private cashdeskState: CashDeskStateService) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
    });
  }

  deactivateExpressMode() {
    this.cashdeskState.resetExpressMode();
  }
}
