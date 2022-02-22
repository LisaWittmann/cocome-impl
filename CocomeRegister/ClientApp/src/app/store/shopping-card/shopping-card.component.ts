import { EventEmitter } from '@angular/core';
import { Component, Output } from '@angular/core';
import { Router } from '@angular/router';
import { OrderElement } from 'src/models/Order';
import { Product } from 'src/models/Product';
import { StoreStateService } from '../store.service';
@Component({
  selector: 'app-store-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.scss'],
})
export class StoreShoppingCardComponent {
  @Output() closeShoppingCardEvent = new EventEmitter<Boolean>();
  shoppingCard: OrderElement[] = [];

  constructor(private storeStateService: StoreStateService, private router: Router) {
    this.storeStateService.currentOrder$.subscribe(currentOrder => {
      this.shoppingCard = currentOrder;
    });
  }

  get totalPrice() {
    if (this.shoppingCard.length === 0) {
      return 0;
    }
    return this.shoppingCard
            .map(element => element.product.price * element.amount)
            .reduce((x, y) => (x + y));
  }

  close = () => this.closeShoppingCardEvent.emit(false);

  addInstance(product: Product) {
    this.storeStateService.addToCard(product);
  }

  updateInstance(product: Product, amount: number, replace = false) {
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
