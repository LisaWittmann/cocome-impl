import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreProductsComponent {
    inventory: Map<Product, number>;
    runningOutOfStock: Product[];

    constructor(private storeStateService: StoreStateService, private router: Router) {
        this.storeStateService.inventory$.subscribe(inventory => {
            this.inventory = inventory;
            this.runningOutOfStock = this.storeStateService.runningOutOfStock;
        });
    }

    addToCard(product: Product) {
        this.storeStateService.addToCard(product);
    }

    addAllToCard(products: Product[]) {
        for (const product of products) {
            this.storeStateService.addToCard(product);
        }
    }

    navigateToDetail(product: Product) {
        this.router.navigate(['/store/product', product.id]);
    }
}
