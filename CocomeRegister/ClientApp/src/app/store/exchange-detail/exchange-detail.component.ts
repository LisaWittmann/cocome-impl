import { Component, Input } from '@angular/core';
import { StockExchange } from 'src/models/StockExchange';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-exchange-detail',
  templateUrl: './exchange-detail.component.html',
  styleUrls: ['./exchange-detail.component.scss'],
})
export class StoreExchangeDetailComponent {
    @Input() exchange: StockExchange;

    constructor(private storeService: StoreStateService) {}

    get status() {
        if (this.exchange.closed) {
            return `Geliefert am ${new Date(this.exchange.deliveringDate).toLocaleDateString('de-DE')}`;
        } else {
            return 'In Bearbeitung';
        }
    }

    get isProvider() {
        return this.storeService.isProvider(this.exchange);
    }

    close() {
        this.storeService.closeExchange(this.exchange);
    }

    startDelivery() {
        this.storeService.startExchange(this.exchange);
    }
}
