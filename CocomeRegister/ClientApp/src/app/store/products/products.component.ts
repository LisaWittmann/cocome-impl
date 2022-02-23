import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/models/Product';
import { StockItem } from 'src/models/Store';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class StoreProductsComponent {
    inventory: StockItem[];
    runningOutOfStock: StockItem[];

    constructor(private storeStateService: StoreStateService, private router: Router) {
        this.storeStateService.inventory$.subscribe(inventory => {
            this.inventory = inventory;
            this.runningOutOfStock = this.storeStateService.runningOutOfStock;
        });
    }

    addToCard(product: Product) {
        this.storeStateService.addToCard(product);
    }

    addAllToCard(items: StockItem[]) {
        for (const item of items) {
            this.storeStateService.addToCard(item.product);
        }
    }

    navigateToDetail(product: Product) {
        this.router.navigate(['/filiale/sortiment', product.id]);
    }
}
