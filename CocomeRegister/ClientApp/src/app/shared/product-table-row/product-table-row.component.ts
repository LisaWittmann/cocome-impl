import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/models/Product';

@Component({
  selector: 'app-product-table-row',
  templateUrl: './product-table-row.component.html',
  styleUrls: ['./product-table-row.component.scss']
})
export class ProductTableRowComponent {
    @Input() product: Product;
    @Input() amount: number | undefined;
    @Input() warn: boolean;
    @Output() selectProductEvent = new EventEmitter<Product>();

    select() {
      this.selectProductEvent.emit(this.product);
    }
}
