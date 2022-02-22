import { Product } from './Product';
import { Store } from './Store';

export class ExchangeElement {
    product: Product;
    amount: number;
}
export class StockExchange {
    id: number;
    elements: ExchangeElement[];
    store: Store;
    provider: Store;
    placingDate: Date;
    deliveringDate: Date;
    closed: boolean;
    sended: boolean;
}
