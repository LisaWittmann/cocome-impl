import { Product } from "./Product";
import { Provider } from "./Provider";
import { Store } from "./Store";
import { Trade, TradeElement } from "./Trade";

export class OrderElement implements TradeElement {
    product: Product;
    amount: number;
}

export class Order implements Trade<Provider> {
    id: number;
    elements: TradeElement[];
    store: Store;
    provider: Provider;
    placingDate: Date;
    deliveringDate: Date;
    closed: boolean;
}
