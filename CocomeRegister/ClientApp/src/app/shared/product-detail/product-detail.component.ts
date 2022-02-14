import { EventEmitter } from '@angular/core';
import { Component, Input, Output } from '@angular/core';
import { Product, Provider } from 'src/services/Models';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent {
  @Input() product: Product = {} as Product;
  @Input() providers: Provider[];
  @Input() restricted: boolean;
  @Output() updateProductEvent = new EventEmitter<Product>();

  uploadImage(files: FileList) {
    const file = files[0];
    console.log(file);
  }

  updateProduct() {
    this.updateProductEvent.emit(this.product);
  }
}
