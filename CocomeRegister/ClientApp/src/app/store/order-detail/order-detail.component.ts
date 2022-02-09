import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Order } from 'src/services/Order';
import { Product } from 'src/services/Product';
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
        this.storeStateService.closeOrder(this.order.id);
    }

    /*getProductPrice(product: Product) {
        return this.order.products.get(product) * product.price;
    }*/

    get status() {
        if (this.order.delivered) {
            return `Geliefert am ${this.order.deliveringDate.toLocaleDateString('de-DE')}`;
        } else if (!this.order.closed) {
            return 'In Bearbeitung';
        }
    }

    get totalPrice() {
        /*let sum = 0;
        for (const [product, amount] of this.order.products) {
            sum += amount * product.price;
        }*/
        return this.order.product.price * this.order.amount;
    }
}
