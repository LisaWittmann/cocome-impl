import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-product-table-row',
  templateUrl: './product-table-row.component.html',
  styleUrls: ['./product-table-row.component.scss']
})
export class ProductTableRowComponent {
    @Input() product: Product;
    @Input() amount: number | undefined;
    @Input() warn: boolean;
    @Input() editable: boolean;
    @Output() selectProductEvent = new EventEmitter<Product>();
    @Output() priceChangeEvent = new EventEmitter<Product>();

    select() {
      this.selectProductEvent.emit(this.product);
    }

    updatePrice() {
      this.priceChangeEvent.emit(this.product);
    }
}
