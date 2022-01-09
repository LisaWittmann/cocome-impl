import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashDeskPaymentComponent {
  expressMode: boolean;

  constructor(
    private cashdeskState: CashDeskStateService,
    private cdr: ChangeDetectorRef
  ) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
      this.cdr.markForCheck();
    })
  }

}