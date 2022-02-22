import { Component, Input } from '@angular/core';
import { StockExchange } from 'src/models/StockExchange';
import { Store } from 'src/models/Store';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-exchange-detail',
  templateUrl: './exchange-detail.component.html',
  styleUrls: ['./exchange-detail.component.scss'],
})
export class StoreExchangeDetailComponent {
    @Input() exchange: StockExchange;
    store: Store;

    constructor(private storeService: StoreStateService) {
        this.storeService.store$.subscribe(store => {
            this.store = store;
        })
    }

    get status() {
        if (this.exchange.closed) {
            return `Geliefert am ${new Date(this.exchange.deliveringDate).toLocaleDateString('de-DE')}`;
        } else {
            return 'In Bearbeitung';
        }
    }

    get isProvider() {
        return this.store && this.store.id === this.exchange.provider.id;
    }

    close() {
        this.storeService.closeExchange(this.exchange);
    }

    startDelivery() {
        this.storeService.startExchange(this.exchange);
    }
}
