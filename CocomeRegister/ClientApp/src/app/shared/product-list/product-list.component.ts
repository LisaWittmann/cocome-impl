import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Product } from 'src/services/Models';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent {
  @Input() discount: number;
  @Input() products: Product[];
  @Output() clickProductEvent = new EventEmitter<Product>();

  filter: string;

  clickProductCard(product: Product) {
    this.clickProductEvent.emit(product);
  }

  setFilter(filter: string) {
    this.filter = filter;
  }

  get displayedProducts() {
    if (!this.filter) {
      return this.products;
    }
    return this.products.filter(p => p.id.toString().includes(this.filter) || p.name.includes(this.filter));
  }
}
