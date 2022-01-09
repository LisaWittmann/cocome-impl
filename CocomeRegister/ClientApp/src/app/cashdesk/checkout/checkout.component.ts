import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { Product } from 'src/services/Product';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashDeskCheckoutComponent {
    expressMode: boolean;
    discount: number;

    constructor(
      private cashdeskState: CashDeskStateService,
    ) {
      this.cashdeskState.expressMode$.subscribe(mode => {
        this.expressMode = mode;
        this.discount = this.cashdeskState.discount;
      })
    } 

    addToCard(product: Product) {
      this.cashdeskState.addProduct(product);
    }

    removeFromCard(product: Product) {
      this.cashdeskState.removeProduct(product);
    }
}