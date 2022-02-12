import { EventEmitter } from '@angular/core';
import { ChangeDetectionStrategy, Component, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.scss'],
})
export class StoreShoppingCardComponent {
  @Output() closeShoppingCardEvent = new EventEmitter<Boolean>();
  shoppingCard: Map<Product, number>;

  constructor(private storeStateService: StoreStateService, private router: Router) {
    this.storeStateService.currentOrder$.subscribe(currentOrder => {
      this.shoppingCard = currentOrder;
    });
  }

  get totalPrice() {
    let sum = 0;
    for (const [product, amount] of this.shoppingCard) {
      sum += amount * product.price;
    }
    return sum;
  }

  close = () => this.closeShoppingCardEvent.emit(false);

  addInstance(product: Product) {
    this.storeStateService.addToCard(product);
  }

  updateInstance(product: Product, amount: number, replace = false) {
    console.log(amount);
    this.storeStateService.addToCard(product, amount, replace);
  }

  removeInstance(product: Product) {
    this.storeStateService.removeFromCard(product);
  }

  removeAll(product: Product) {
    this.storeStateService.removeAllFromCard(product);
  }

  confirm() {
    this.storeStateService.placeNewOrder();
    this.router.navigate(['/filiale/bestellungen']);
    this.closeShoppingCardEvent.emit(false);
  }
}
