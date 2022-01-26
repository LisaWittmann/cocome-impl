import { Component } from '@angular/core';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss'],
})
export class StoreInventoryComponent {
    inventory: Map<Product, number>;

    constructor(private storeState: StoreStateService) {
        this.storeState.inventory$.subscribe(inventory => {
            this.inventory = inventory;
        })
    }
}