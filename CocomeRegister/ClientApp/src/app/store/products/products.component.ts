import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product, StockItem } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
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
        this.router.navigate(['/filiale/produkt', product.id]);
    }
}
