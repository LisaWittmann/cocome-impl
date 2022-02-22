import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Product } from 'src/models/Product';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent {
  @Input() discount: number;
  @Input() products: Product[];
  @Output() clickProductEvent = new EventEmitter<Product>();
  @Output() filterEvent = new EventEmitter<string>();

  filter: string;

  clickProductCard(product: Product) {
    this.clickProductEvent.emit(product);
  }

  setFilter(filter: string) {
    this.filter = filter;
    this.filterEvent.emit(filter);
  }
}
