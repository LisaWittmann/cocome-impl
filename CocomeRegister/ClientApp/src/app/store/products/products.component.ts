import { Component } from '@angular/core';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'store-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class StoreProductsComponent {
    inventory: Map<Product, number>;
    runningOutOfStock: Product[];

    constructor(private storeStateService: StoreStateService) {
        this.storeStateService.inventory$.subscribe(inventory => {
            this.inventory = inventory;
            this.runningOutOfStock = this.storeStateService.runningOutOfStock;
        })
    }

    addToCard(product: Product) {
        this.storeStateService.addToCard(product);
    }
}