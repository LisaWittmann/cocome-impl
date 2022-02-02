import { Component } from '@angular/core';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'store-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss'],
})
export class StoreInventoryComponent {
    inventory: Map<Product, number>;
    runningOutOfStock: Product[];

    constructor(private storeState: StoreStateService) {
        this.storeState.inventory$.subscribe(inventory => {
            this.inventory = inventory;
            this.runningOutOfStock = this.storeState.runningOutOfStock;
        })
    }
}