import { Component } from '@angular/core';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
})
export class StoreComponent {
  shoppingCardOpen = false;

  toggleShoppingCard(open: boolean) {
    this.shoppingCardOpen = open;
  }
}
