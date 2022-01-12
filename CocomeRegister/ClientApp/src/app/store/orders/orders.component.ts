import { Component } from '@angular/core';
import { Order } from 'src/services/Order';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class StoreOrdersComponent {
    orders: Order[];

    openOrders: Order[];
    closedOrders: Order[];
    pendingOrders: Order[];

    constructor(private storeState: StoreStateService) {
        this.storeState.orders$.subscribe(orders => {
            this.orders = orders;
            this.openOrders = this.storeState.openOrders;
            this.closedOrders = this.storeState.closedOrders;
            this.pendingOrders = this.storeState.pendingOrders;
        })
    }

    closeOrder = (orderId: number) => this.storeState.closeOrder(orderId);
}