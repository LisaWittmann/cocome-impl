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
    shoppingCard = new Array<Product>();
    filter: string;

    constructor(
      private cashdeskState: CashDeskStateService,
      private cdr: ChangeDetectorRef
    ) {
      this.cashdeskState.expressMode$.subscribe(mode => {
        this.expressMode = mode;
        this.cdr.markForCheck();
      })
      this.cashdeskState.discount$.subscribe(discount => {
        this.discount = discount;
        this.cdr.markForCheck();
      })
    } 

    addToCard(product: Product) {
      if (this.expressMode && this.shoppingCard.length >= 8) return;
      this.shoppingCard.push(product);
    }

    removeFromCard(product: Product) {
      this.shoppingCard = this.shoppingCard.filter(p => p.id != product.id);
    } 

    setFilter(filter: string) {
      this.filter = filter;
    }

    resetExpressMode() {
      this.expressMode = false;
      this.discount = 0;
    }

}