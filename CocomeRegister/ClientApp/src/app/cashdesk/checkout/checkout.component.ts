import { ChangeDetectionStrategy, Component } from '@angular/core';
import { StoreStateService } from 'src/app/store/store.service';
import { Product } from 'src/services/Product';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CashDeskCheckoutComponent {
    expressMode: boolean;
    discount: number;
    products: Product[];

    constructor(
      private cashdeskState: CashDeskStateService,
      private storeState: StoreStateService,
    ) {
      this.cashdeskState.expressMode$.subscribe(mode => {
        this.expressMode = mode;
        this.discount = this.cashdeskState.discount;
      });
      this.storeState.inventory$.subscribe(() => {
        this.products = this.storeState.availableProducts;
      });
    }

    addToCard(product: Product) {
      this.cashdeskState.addProduct(product);
    }

    removeFromCard(product: Product) {
      this.cashdeskState.removeProduct(product);
    }
}
