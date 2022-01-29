import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Order } from 'src/services/Order';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreOrdersComponent {
    orders: Order[];

    constructor(private storeState: StoreStateService) {
        this.storeState.orders$.subscribe(orders => {
            this.orders = orders;
        })
    }

    get closedOrders() {
        return this.orders.filter(order => order.closed);
    }

    get pendingOrders() {
        return this.orders.filter(order => (order.delivered && !order.closed));
    }

    get openOrders() {
        return this.orders.filter(order => !order.delivered);
    }

    title = (order: Order) => {
        return `Bestellung ${order.id} vom 
                ${order.placingDate.toLocaleDateString('de-DE')}`;
    }
}