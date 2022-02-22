import { EventEmitter } from '@angular/core';
import { Component, Output } from '@angular/core';
import { OrderElement } from 'src/models/Order';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss'],
})
export class StoreNavigationComponent {
  @Output() openShoppingCardEvent = new EventEmitter<Boolean>();
  shoppingCard: OrderElement[];

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.currentOrder$.subscribe(currentOrder => {
      this.shoppingCard = currentOrder;
    });
  }

  get shoppingCardItems() {
    return this.shoppingCard.length;
  }

  openShoppingCard = () => this.openShoppingCardEvent.emit(true);
}
