import { Component, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent {
  product: Product;

  constructor(
    private storeStateService: StoreStateService,
    private router: Router,
    private location: Location
  ) {
    const productId = Number(router.url.split('/').pop());
    this.storeStateService.inventory$.subscribe(inventory => {
      this.product = [...inventory.keys()].find(p => p.id == productId);
    })
    if (!this.product) {
      this.location.back();
    }
  }

  uploadImage(files: FileList) {
    const file = files[0];
    console.log(file);
  }

  updateProduct() {
    this.storeStateService.updateProduct(this.product);
  }

  addToCard() {
    this.storeStateService.addToCard(this.product);
  }
}
