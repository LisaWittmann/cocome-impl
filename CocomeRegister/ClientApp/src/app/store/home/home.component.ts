import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Order } from 'src/services/Order';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'store-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreHomeComponent {
  runningOutOfStock: Product[];
  orders: Order[];

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.inventory$.subscribe(() => {
      this.runningOutOfStock = this.storeStateService.runningOutOfStock;
    })
    this.storeStateService.orders$.subscribe(orders => {
      this.orders = orders
    })
  }

  get pendingOrders() {
    return this.orders.filter(order => order.delivered && !order.closed)
  }

  title = (order: Order) => {
    return `Bestellung ${order.id} vom 
            ${order.placingDate.toLocaleDateString('de-DE')}`;
  }
}
