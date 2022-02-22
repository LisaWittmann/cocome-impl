import { Component } from '@angular/core';
import { Order } from 'src/models/Order';
import { StockExchange } from 'src/models/StockExchange';
import { Trade } from 'src/models/Trade';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class StoreOrdersComponent {
    orders: Order[];
    providedExchanges: StockExchange[];
    exchanges: StockExchange[];

    constructor(private storeService: StoreStateService) {
        this.storeService.orders$.subscribe(orders => {
            this.orders = orders;
        });
        this.storeService.exchanges$.subscribe(exchanges => {
            this.exchanges = exchanges.filter(ex => !this.storeService.isProvider(ex) && !ex.closed);
            this.providedExchanges = exchanges.filter(ex => this.storeService.isProvider(ex) && !ex.sended);
        });
    }

    title = (trade: Trade<any>) => {
        return `Bestellung ${trade.id} vom
                ${new Date(trade.placingDate).toLocaleDateString('de-DE')}`;
    }
}
