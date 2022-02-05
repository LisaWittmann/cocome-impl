import { Component } from '@angular/core';

@Component({
  selector: 'app-store-root',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss'],
})
export class StoreComponent {
  shoppingCardOpen = false;

  toggleShoppingCard(open: boolean) {
    this.shoppingCardOpen = open;
  }
}
