import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss'],
})
export class StoreInventoryComponent {
    inventory: Map<Product, number>;
    runningOutOfStock: Product[];

    constructor(private storeState: StoreStateService, private router: Router) {
        this.storeState.inventory$.subscribe(inventory => {
            this.inventory = inventory;
            this.runningOutOfStock = this.storeState.runningOutOfStock;
        })
    }

    redirectToShopping(product: Product) {
        this.router.navigate([ 'store/shopping' ], { fragment: product.id.toString() });
    }
}