import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Product } from 'src/services/Models';
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

  uploadImage(files: FileList) {
    const file = files[0];
    console.log(file);
  }

  updateProduct() {
    this.storeStateService.updateProduct(this.product);
    this.router.navigate(['/filiale/sortiment']);
  }

  addToCard() {
    this.storeStateService.addToCard(this.product);
  }
}
