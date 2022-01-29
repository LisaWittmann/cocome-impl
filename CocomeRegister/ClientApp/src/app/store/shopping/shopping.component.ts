import { ChangeDetectionStrategy, Component } from "@angular/core";
import { Product } from "src/services/Product";
import { StoreStateService } from "../store.service";

@Component({
    selector: 'app-store-shopping',
    templateUrl: './shopping.component.html',
    styleUrls: ['./shopping.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreShoppingComponent {
    products: Product[];

    constructor(private storeState: StoreStateService) {
        this.storeState.inventory$.subscribe(inventory => {
            this.products = [...inventory.keys()];
        })
    }
    
}