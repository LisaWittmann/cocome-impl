import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { StoreStateService } from '../store.service';
import { Product } from 'src/models/Product';

@Component({
  selector: 'app-store-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class StoreProductComponent {
  product = {} as Product;

  constructor(
    private storeStateService: StoreStateService,
    private router: Router,
    private location: Location,
  ) {
    const productId = Number(router.url.split('/').pop());
    this.storeStateService.getProduct(productId).subscribe(result => {
      this.product = result;
    }, error => {
      console.error(error);
      this.location.back();
    });
  }

  updateProduct() {
    this.storeStateService.updateProduct(this.product);
    this.router.navigate(['/filiale/sortiment']);
  }

  addToCard() {
    this.storeStateService.addToCard(this.product);
  }
}
