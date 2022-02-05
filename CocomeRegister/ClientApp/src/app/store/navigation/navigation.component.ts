import { EventEmitter } from '@angular/core';
import { Component, Input, Output } from '@angular/core';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class StoreNavigationComponent {
  @Output() openShoppingCardEvent = new EventEmitter<Boolean>();
  shoppingCard: Map<Product, Number>;

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.currentOrder$.subscribe(currentOrder => {
      this.shoppingCard = currentOrder;
    });
  }

  get shoppingCardItems() {
    return this.shoppingCard.size;
  }

  openShoppingCard = () => this.openShoppingCardEvent.emit(true);
}
