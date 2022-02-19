import { Component } from '@angular/core';
import { Order } from 'src/services/Models';
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

    constructor(private storeState: StoreStateService) {
        this.storeState.orders$.subscribe(orders => {
            this.orders = orders;
            this.closedOrders = orders.filter(order => order.closed);
            this.openOrders = orders.filter(order => !order.closed);
        });
    }

    title = (order: Order) => {
        return `Bestellung ${order.id} vom
                ${new Date(order.placingDate).toLocaleDateString('de-DE')}`;
    }
}
