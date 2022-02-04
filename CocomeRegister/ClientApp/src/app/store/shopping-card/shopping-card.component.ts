import { EventEmitter } from "@angular/core";
import { ChangeDetectionStrategy, Component, Output } from "@angular/core";
import { Router } from "@angular/router";
import { Product } from "src/services/Product";
import { StoreStateService } from "../store.service";

@Component({
  selector: 'store-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreShoppingCardComponent {
  @Output() closeShoppingCardEvent = new EventEmitter<Boolean>()
  close = () => this.closeShoppingCardEvent.emit(false);

  shoppingCard: Map<Product, Number>;

  constructor(private storeStateService: StoreStateService, private router: Router) {
    this.storeStateService.currentOrder$.subscribe(currentOrder => {
      this.shoppingCard = currentOrder;
    })
  }

  addInstance(product: Product) {
    this.storeStateService.addToCard(product);
  }

  removeInstance(product: Product) {
    this.storeStateService.removeFromCard(product);
  }

  removeAll(product: Product) {
    this.storeStateService.removeAllFromCard(product);
  }

  confirm() {
    this.storeStateService.placeNewOrder();
    this.router.navigate(['/store/orders']);
    this.closeShoppingCardEvent.emit(false);
  }
}