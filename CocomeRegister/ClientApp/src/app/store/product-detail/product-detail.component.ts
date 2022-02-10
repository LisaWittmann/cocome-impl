import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class StoreProductDetailComponent {
  product: Product;
  create: boolean;

  constructor(
    private storeStateService: StoreStateService,
    private router: Router,
    private location: Location
  ) {
    const productId = Number(router.url.split('/').pop());
    this.storeStateService.inventory$.subscribe(inventory => {
      this.product = inventory.find(item => item.product.id === productId).product;
      console.log(this.product);
    });
    if (!this.product) {
      this.product = {} as Product;
      this.create = true;
    }
  }

  uploadImage(files: FileList) {
    const file = files[0];
    console.log(file);
  }

  updateProduct() {
    if (this.create) {
      return;
    }
    this.storeStateService.updateInventory();
  }

  addToCard() {
    this.storeStateService.addToCard(this.product);
  }

  createProduct() {
    this.storeStateService.createProduct(this.product);
  }
}
