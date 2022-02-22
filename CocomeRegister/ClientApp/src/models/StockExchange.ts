import { Product } from "./Product";
import { Store } from "./Store";
import { TradeElement, Trade } from "./Trade";

export class ExchangeElement implements TradeElement {
    product: Product;
    amount: number;
}
export class StockExchange implements Trade<Store> {
    id: number;
    elements: TradeElement[];
    store: Store;
    provider: Store;
    placingDate: Date;
    deliveringDate: Date;
    closed: boolean;
    sended: boolean;
}