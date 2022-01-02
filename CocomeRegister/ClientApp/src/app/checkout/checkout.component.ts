import { Component } from '@angular/core';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {
  shoppingCard = new Array<Product>();
  filter: string;

  addToCard(product: Product) {
    this.shoppingCard.push(product);
  }

  removeFromCard(product: Product) {
    this.shoppingCard = this.shoppingCard.filter(p => p.id != product.id);
  }

  setFilter(filter: string) {
    this.filter = filter;
  }

}