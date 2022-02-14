import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from 'src/services/Models';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class StoreProductComponent {
  product: Product;

  constructor(
    private storeStateService: StoreStateService,
    private router: Router,
    private location: Location,
  ) {
    const productId = Number(router.url.split('/').pop());
    this.storeStateService.getProduct(productId).subscribe(result => {
      this.product = result
    }, error => {
      console.error(error);
      this.location.back()
    });
  }

  updateProduct() {
    this.storeStateService.updateProduct(this.product);
  }

  addToCard() {
    this.storeStateService.addToCard(this.product);
  }
}
