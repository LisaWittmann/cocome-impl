import { Component, Input } from '@angular/core';
import { Order, OrderElement } from 'src/models/Order';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss'],
})
export class StoreOrderDetailComponent {
    @Input() order: Order;

    constructor(private storeStateService: StoreStateService) {}

    close() {
        this.storeStateService.closeOrder(this.order);
    }

    getElementPrice(element: OrderElement) {
        return element.amount * element.product.price;
    }

    get status() {
        if (this.order.closed) {
            return `Geliefert am ${new Date(this.order.deliveringDate).toLocaleDateString('de-DE')}`;
        } else {
            return 'In Bearbeitung';
        }
    }

    get totalPrice() {
        if (this.order.elements.length == 0) return 0;
        return this.order.elements
            .map(element => element.product.price * element.amount)
            .reduce((x, y) => (x + y));
    }
}
