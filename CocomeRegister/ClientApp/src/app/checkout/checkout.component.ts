import { Component } from '@angular/core';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {
    shoppingCard = new Array<Product>();

    addToCard(product: Product) {
        this.shoppingCard.push(product);
    }

}