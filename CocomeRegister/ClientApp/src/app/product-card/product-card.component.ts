import { Component, Input } from '@angular/core';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {
  @Input() product: Product;
  @Input() discount: number;

  getDiscountedPrice() {
    if (this.discount) {
      const percentage = 1 - this.discount;
      return (percentage * this.product.price).toFixed(2);
    }
  }
}
