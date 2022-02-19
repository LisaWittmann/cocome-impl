import { Component, Input } from '@angular/core';
import { OrderElement, Order } from 'src/services/Models';
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
        if (this.order.orderElements.length == 0) return 0;
        return this.order.orderElements
            .map(element => element.product.price * element.amount)
            .reduce((x, y) => (x + y));
    }
}
